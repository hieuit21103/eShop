namespace Catalog.API.Domain.Specifications.CategorySpecification;

public class CategoryByNameSpecification : Specification<Category>
{
    public CategoryByNameSpecification(string name)
    {
        Query.Where(category => category.Name == name);
    }
}