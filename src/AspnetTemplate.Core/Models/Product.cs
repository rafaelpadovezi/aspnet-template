namespace AspnetTemplate.Core.Models;

public class Product : Entity
{
    public string Code { get; set; } = "";
    public List<Link> Photos { get; set; } = new();
    public List<ProductAttribute> Attributes { get; set; } = new();
}

public class Link
{
    public string Value { get; set; } = "";
    public static implicit operator string(Link tag) => tag.Value;
    public static implicit operator Link(string tag) => new() { Value = tag };
}

public class ProductAttribute : Entity
{
    public string Key { get; set; } = "";
    public string Value { get; set; } = "";
}