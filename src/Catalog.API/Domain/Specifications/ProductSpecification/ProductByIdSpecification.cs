namespace Catalog.API.Domain.Specifications.ProductSpecification;

public class ProductByIdSpecification : Specification<Product>
{
    public ProductByIdSpecification(Guid id)
    {
        Query.Where(p => p.Id == id)
             .Include(p => p.Brand)
             .Include(p => p.Category)
             .Include(p => p.ProductImages);
    }
}
