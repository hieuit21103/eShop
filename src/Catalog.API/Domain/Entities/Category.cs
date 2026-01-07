namespace Catalog.API.Domain.Entities;

public class Category
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();
}