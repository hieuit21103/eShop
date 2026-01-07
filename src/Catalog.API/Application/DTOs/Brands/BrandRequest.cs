namespace Catalog.API.Application.DTOs.Brands;

public class BrandRequest
{
    public string Name { get; set; } = string.Empty;
    public IFormFile? Image { get; set; }
}
