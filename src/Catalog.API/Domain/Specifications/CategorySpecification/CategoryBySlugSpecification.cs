namespace Catalog.API.Domain.Specifications.CategorySpecification;

public class CategoryBySlugSpecification : Specification<Category>
{
    public CategoryBySlugSpecification(string slug)
    {
        Query.Where(category => category.Slug == slug);
    }
}