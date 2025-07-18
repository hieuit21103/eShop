namespace Catalog.API.Models;

public class Product : IEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public string Description { get; set; }
    public string ShortDescription { get; set; }
    public decimal UnitPrice { get; set; }
    public string Discount { get; set; }
    public int StockQuantity { get; set; }
    public Guid CategoryId { get; set; }
    public Guid BrandId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(7);
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow.AddHours(7);

    [ForeignKey("CategoryId")]
    public Category? Category { get; set; }
    [ForeignKey("BrandId")]
    public Brand? Brand { get; set; }
    public ICollection<ProductImages> ProductImages { get; set; } = new List<ProductImages>();
}