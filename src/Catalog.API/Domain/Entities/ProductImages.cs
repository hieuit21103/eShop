namespace Catalog.API.Domain.Entities;

public class ProductImages
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string ImageId { get; set; } = null!;
    public bool IsPrimary { get; set; }
    public Product Product { get; set; } = new Product();
}