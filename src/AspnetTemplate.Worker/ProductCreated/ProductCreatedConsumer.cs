using DotNetCore.CAP;
using Ziggurat;

namespace AspnetTemplate.Worker.ProductCreated;

public class ProductCreatedConsumer : ICapSubscribe
{
    private readonly IConsumerService<ProductCreatedMessage> _service;

    public ProductCreatedConsumer(IConsumerService<ProductCreatedMessage> service)
    {
        _service = service;
    }

    [CapSubscribe("product.created", Group = "product.created")]
    public async Task ProcessMessageAsync(ProductCreatedMessage message)
    {
        await _service.ProcessMessageAsync(message);
    }
}
