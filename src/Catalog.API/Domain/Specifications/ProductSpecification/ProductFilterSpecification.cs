namespace Catalog.API.Domain.Specifications.ProductSpecification;

public class ProductFilterSpecification : Specification<Product>
{
    public ProductFilterSpecification(ProductFilterParams filterParams)
    {
        Query.Include(p => p.Brand)
             .Include(p => p.Category)
             .Include(p => p.ProductImages);

        if (filterParams.BrandId.HasValue)
            Query.Where(p => p.Brand.Id == filterParams.BrandId);

        if (filterParams.CategoryId.HasValue)
            Query.Where(p => p.Category.Id == filterParams.CategoryId);

        if (filterParams.MinPrice.HasValue)
            Query.Where(p => p.UnitPrice >= filterParams.MinPrice);

        if (filterParams.MaxPrice.HasValue)
            Query.Where(p => p.UnitPrice <= filterParams.MaxPrice);

        if (!string.IsNullOrEmpty(filterParams.Search))
            Query.Where(p => p.Name.Contains(filterParams.Search) || p.Description.Contains(filterParams.Search));

        // Sorting
        if (!string.IsNullOrEmpty(filterParams.SortBy))
        {
            switch (filterParams.SortBy.ToLower())
            {
                case "price":
                    if (filterParams.IsDescending)
                        Query.OrderByDescending(p => p.UnitPrice);
                    else
                        Query.OrderBy(p => p.UnitPrice);
                    break;
                case "name":
                default:
                    if (filterParams.IsDescending)
                        Query.OrderByDescending(p => p.Name);
                    else
                        Query.OrderBy(p => p.Name);
                    break;
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
