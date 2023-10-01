﻿using Asp.Versioning;

using AspnetTemplate.Core.Database;
using AspnetTemplate.Core.Dtos;
using AspnetTemplate.Core.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop.Infrastructure;

namespace AspnetTemplate.Api.Controllers.v1;

[ApiVersion("1")]
[ApiController]
[Route("v{version:apiVersion}/[controller]")]
public class ProductController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProductController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var product = await _context.Products
            .AsNoTracking()
            .Include(x => x.Attributes)
            .SingleOrDefaultAsync(x => x.Id == id);

        if (product is null)
            return NotFound();
        return Ok(ProductDto.FromProduct(product));
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ProductDto dto)
    {
        var product = new Product
        {
            Code = dto.Code,
            Attributes = dto.Attributes
                .Select(x => new ProductAttribute { Key = x.Key, Value = x.Value })
                .ToList(),
            Photos = dto.Photos.Select(x => (Link)x).ToList()
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return Ok(ProductDto.FromProduct(product));
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] ProductsParameters parameters)
    {
        var query = _context.Products.AsNoTracking().Include(x => x.Attributes).AsQueryable();

        if (!string.IsNullOrWhiteSpace(parameters.Code))
            query = query.Where(x => x.Code.Contains(parameters.Code));
        if (!string.IsNullOrWhiteSpace(parameters.AttributeKey))
            query = query.Where(x => x.Attributes.Any(attr => attr.Key == parameters.AttributeKey));
        if (!string.IsNullOrWhiteSpace(parameters.AttributeValue))
            query = query.Where(
                x => x.Attributes.Any(attr => attr.Value == parameters.AttributeValue)
            );

        var total = await query.CountAsync();
        var products = await query
            .OrderBy(x => x.Code)
            .Skip(parameters.Offset)
            .Take(parameters.Limit)
            .ToListAsync();
        return Ok(new Paginated<ProductDto>(total, products.Select(ProductDto.FromProduct)));
    }
}

public record Paginated<T>(int Count, IEnumerable<T> Results);

public record ProductsParameters(
    string? Code,
    string? AttributeKey,
    string? AttributeValue,
    int Limit = 5,
    int Offset = 0
);
