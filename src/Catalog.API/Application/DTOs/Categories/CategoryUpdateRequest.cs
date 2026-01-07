namespace Catalog.API.Application.DTOs.Categories;

public class CategoryUpdateRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
