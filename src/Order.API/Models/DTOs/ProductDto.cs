namespace Order.API.Models.DTOs;

public class ProductDto
{
    public string Name { get; set; }
    public string Slug { get; set; }
    public string Description { get; set; }
    public string ShortDescription { get; set; }
    public decimal UnitPrice { get; set; }
    public string Discount { get; set; }
    public int StockQuantity { get; set; }
}