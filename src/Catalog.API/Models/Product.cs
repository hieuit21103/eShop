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
    public DateTime CreatedAt { get; set; } = GetVietnamTimeNow();
    public DateTime UpdatedAt { get; set; } = GetVietnamTimeNow();

    [ForeignKey("CategoryId")]
    public Category? Category { get; set; }
    [ForeignKey("BrandId")]
    public Brand? Brand { get; set; }
    public ICollection<ProductImages> ProductImages { get; set; } = new List<ProductImages>();

    private static DateTime GetVietnamTimeNow()
    {
        var timeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
        return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);
    }
}