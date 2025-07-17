namespace Catalog.API.Models;

public class ProductImages : IEntity
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string PictureUrl { get; set; }
    public bool IsPrimary { get; set; }

    [ForeignKey("ProductId")]
    public Product Product { get; set; }
}