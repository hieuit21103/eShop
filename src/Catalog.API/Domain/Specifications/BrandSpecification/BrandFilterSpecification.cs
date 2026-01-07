namespace Catalog.API.Domain.Specifications.BrandSpecification;

public class BrandFilterSpecification : Specification<Brand>
{
    public BrandFilterSpecification(BrandFilterParams filterParams)
    {
        if (!string.IsNullOrEmpty(filterParams.Search))
        {
            Query.Where(b => b.Name.Contains(filterParams.Search));
        }

        if (!string.IsNullOrEmpty(filterParams.SortBy))
        {
            if (filterParams.IsDescending)
            {
                Query.OrderByDescending(b => b.Name); // Default to Name, or use dynamic
            }
            else
            {
                Query.OrderBy(b => b.Name);
            }
        }

        if (filterParams.PageSize > 0)
        {
            Query.Skip((filterParams.PageNumber - 1) * filterParams.PageSize)
                 .Take(filterParams.PageSize);
        }
    }
}
