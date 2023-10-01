using AspnetTemplate.Core.Database;
using Ziggurat;

namespace AspnetTemplate.Worker.ProductCreated;

public class ProductCreatedService : IConsumerService<ProductCreatedMessage>
{
    private readonly AppDbContext _context;

    public ProductCreatedService(AppDbContext context)
    {
        _context = context;
    }

    public async Task ProcessMessageAsync(ProductCreatedMessage message)
    {
        // Do something
        await _context.SaveChangesAsync();
    }
}
