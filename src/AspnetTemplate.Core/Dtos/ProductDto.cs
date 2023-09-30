using AspnetTemplate.Core.Models;

namespace AspnetTemplate.Core.Dtos;

public record ProductDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = "";
    public List<string> Photos { get; set; } = new();
    public List<ProductAttributeDto> Attributes { get; set; } = new();
    public string Name { get; set; } = "";

    public static ProductDto FromProduct(Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Code = product.Code,
            Attributes =
                product.Attributes.Select(attr => new ProductAttributeDto { Key = attr.Key, Value = attr.Value })
                    .ToList(),
            Photos = product.Photos.Select(x => (string)x).ToList()
        };
    }
}

public class ProductAttributeDto
{
    public string Key { get; set; } = "";
    public string Value { get; set; } = "";
}