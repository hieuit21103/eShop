namespace Catalog.API.Application.DTOs.Products;

public class ProductResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public string? Discount { get; set; }
    public int StockQuantity { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;

    public Guid BrandId { get; set; }
    public string BrandName { get; set; } = string.Empty;

    public int ImageCount { get; set; }
    public List<ProductImageResponse> ProductImages { get; set; } = new();
}

public class ProductImageResponse
{
    public Guid Id { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public bool IsPrimary { get; set; }
}
