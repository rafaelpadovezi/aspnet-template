using Asp.Versioning;

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
            Attributes = dto.Attributes.Select(x => new ProductAttribute
            {
                Key = x.Key,
                Value = x.Value
            }).ToList(),
            Photos = dto.Photos.Select(x => (Link)x).ToList()
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return Ok(ProductDto.FromProduct(product));
    }
}