namespace Catalog.API.Models;

public class Brand : IEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string PictureUrl { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();
}