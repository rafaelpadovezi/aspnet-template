using AspnetTemplate.Api.Controllers.v1;
using AspnetTemplate.Core.Dtos;
using AspnetTemplate.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace AspnetTemplate.Tests.Controllers.v1;

public class ProductControllerTests_Get : IClassFixture<AppFixture>
{
    private readonly AppFixture _factory;
    private const string Endpoint = "v1/product";

    public ProductControllerTests_Get(AppFixture factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Get_ShouldGetProducts()
    {
        var client = _factory.CreateClient();
        _factory.DbContext.Products.AddRange(
            new List<Product>
            {
                new()
                {
                    Code = "2222",
                    Name = "Shiny product",
                    Attributes = new List<ProductAttribute>
                    {
                        new() { Key = "key", Value = "value" }
                    },
                    Photos = new List<Link> { "https://localhost/some-photo" }
                },
                new()
                {
                    Code = "3333",
                    Name = "Nice product",
                    Attributes = new List<ProductAttribute>
                    {
                        new() { Key = "other-key", Value = "other-value" }
                    },
                    Photos = new List<Link> { "https://localhost/some-photo" }
                }
            }
        );
        await _factory.DbContext.SaveChangesAsync();

        var response = await client.GetFromJsonAsync<Paginated<ProductDto>>($"{Endpoint}?code=222");

        Assert.NotNull(response);
        Assert.Equal(1, response.Count);
        Assert.Collection(response.Results, item => Assert.Equal("2222", item.Code));
    }
}

public class ProductControllerTests_Post : IClassFixture<TransactionalAppFixture>
{
    private readonly TransactionalAppFixture _factory;
    private readonly DbSet<Product> _productsDb;
    private const string Endpoint = "v1/product";

    public ProductControllerTests_Post(TransactionalAppFixture factory)
    {
        _factory = factory;
        _productsDb = factory.DbContext.Products;
    }

    [Fact]
    public async Task Post_NewProduct_ShouldAddToDb()
    {
        var client = _factory.CreateClient();
        var productDto = new ProductDto
        {
            Code = "1111",
            Name = "Shiny product",
            Attributes = new List<ProductAttributeDto>
            {
                new() { Key = "key", Value = "value" }
            },
            Photos = new List<string> { "https://localhost/some-photo" }
        };

        var response = await client.PostAsJsonAsync(Endpoint, productDto);

        await response.FailIfNotSuccess();
        var addedProduct = await response.Content.ReadFromJsonAsync<ProductDto>();
        var dbProduct = _productsDb.SingleOrDefault(x => addedProduct!.Id == x.Id);
        Assert.Equal("1111", dbProduct!.Code);
    }
}
