namespace Catalog.API.Domain.Specifications.BrandSpecification;

public class BrandBySlugSpecification : Specification<Brand>
{
    public BrandBySlugSpecification(string slug)
    {
        Query.Where(brand => brand.Slug == slug);
    }
}