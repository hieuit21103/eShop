namespace Catalog.API.Domain.Entities;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string ShortDescription { get; set; } = null!;
    public decimal UnitPrice { get; set; }
    public string Discount { get; set; } = null!;
    public int StockQuantity { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(7);
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow.AddHours(7);
    public Category? Category { get; set; }
    public Brand? Brand { get; set; }
    public ICollection<ProductImages> ProductImages { get; set; } = new List<ProductImages>();
}