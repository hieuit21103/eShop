namespace Catalog.API.Application.DTOs.Brands;

public class BrandUpdateRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public IFormFile? Picture { get; set; }
}
