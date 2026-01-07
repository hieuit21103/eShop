namespace Catalog.API.Domain.Specifications.ProductSpecification;

public class ProductBySlugSpecification : Specification<Product>
{
    public ProductBySlugSpecification(string slug)
    {
        Query.Where(p => p.Slug == slug)
             .Include(p => p.Brand)
             .Include(p => p.Category)
             .Include(p => p.ProductImages);
    }
}
