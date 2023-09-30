using System.Net.Http.Json;

using AspnetTemplate.Core.Database;
using AspnetTemplate.Core.Dtos;
using AspnetTemplate.Tests.Support;

using Microsoft.Extensions.DependencyInjection;

using Xunit;

namespace AspnetTemplate.Tests.Controllers;

public class ProductControllerTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly TestWebApplicationFactory _factory;

    public ProductControllerTests(TestWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Post_ShouldSaveProduct()
    {
        var context = _factory.Services.GetRequiredService<AppDbContext>();
        var transaction = await context.Database.BeginTransactionAsync();

        var client = _factory.CreateClient();
        var productDto = new ProductDto
        {
            Code = "1234",
            Name = "Shiny product",
            Attributes = new List<ProductAttributeDto> { new() { Key = "" } }
        };

        var result = await client.PostAsJsonAsync("v1/product", productDto);

        var addedProduct = await result.Content.ReadFromJsonAsync<ProductDto>();

        var dbProduct = context.Products.SingleOrDefault(x => addedProduct!.Id == x.Id);
        Assert.Equal("1234", dbProduct!.Code);
        await transaction.RollbackAsync();
    }

}