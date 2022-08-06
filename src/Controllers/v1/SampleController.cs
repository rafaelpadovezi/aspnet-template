using AspnetTemplate.Core.Models;
using AspnetTemplate.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspnetTemplate.Controllers.v1;

[ApiVersion("1.0")]
[ApiController]
[Route("v{version:apiVersion}/[controller]")]
public class SampleController : ControllerBase
{
    private readonly AppDbContext _context;

    public SampleController(AppDbContext context)
    {
        _context = context;
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var samples = await _context.Samples.SingleOrDefaultAsync(x => x.Id == id);
        if (samples is null)
            return NotFound();
        return Ok(samples);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var samples = await _context.Samples.ToListAsync();
        return Ok(samples);
    }

    [HttpPost]
    public async Task<IActionResult> Post(Sample sample)
    {
        _context.Add(sample);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new {id = sample.Id, version = "1.0"}, sample);
    }
}