namespace Catalog.API.Domain.Specifications.BrandSpecification;

public class BrandByNameSpecification : Specification<Brand>
{
    public BrandByNameSpecification(string name)
    {
        Query.Where(brand => brand.Name == name);
    }
}
