namespace Catalog.API.Domain.Specifications.CategorySpecification;

public class CategoryFilterSpecification : Specification<Category>
{
    public CategoryFilterSpecification(CategoryFilterParams filterParams)
    {
        if (!string.IsNullOrEmpty(filterParams.Search))
        {
            Query.Where(c => c.Name.Contains(filterParams.Search));
        }

        // Sorting
        if (!string.IsNullOrEmpty(filterParams.SortBy))
        {
            if (filterParams.IsDescending)
            {
                Query.OrderByDescending(c => c.Name);
            }
            else
            {
                Query.OrderBy(c => c.Name);
            }
        }

        // Pagination
        if (filterParams.PageSize > 0)
        {
            Query.Skip((filterParams.PageNumber - 1) * filterParams.PageSize)
                 .Take(filterParams.PageSize);
        }
    }
}
