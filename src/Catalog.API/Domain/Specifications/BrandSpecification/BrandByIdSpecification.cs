namespace Catalog.API.Domain.Specifications.BrandSpecification;

public class BrandByIdSpecification : Specification<Brand>
{
    public BrandByIdSpecification(Guid id)
    {
        Query.Where(b => b.Id == id);
    }
}
