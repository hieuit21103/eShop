namespace Catalog.API.Models;

public class Category : IEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();
}