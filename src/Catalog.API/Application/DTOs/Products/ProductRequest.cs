namespace Catalog.API.Application.DTOs.Products;

public class ProductRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public string? Discount { get; set; }
    public int StockQuantity { get; set; }
    public Guid? CategoryId { get; set; }
    public Guid? BrandId { get; set; }
    public List<IFormFile> Images { get; set; } = new();
}
