namespace Catalog.API.Domain.Specifications.CategorySpecification;

public class CategoryByIdSpecification : Specification<Category>
{
    public CategoryByIdSpecification(Guid id)
    {
        Query.Where(c => c.Id == id);
    }
}
