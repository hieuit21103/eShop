using Ardalis.Specification;

namespace Identity.API.Domain.Specifications;

public class UserAddressFilterSpecification : Specification<UserAddress>
{
    public UserAddressFilterSpecification(UserAddressFilterParams filter, bool isPaging = false)
    {
        if (filter.UserId.HasValue)
        {
            Query.Where(ua => ua.UserId == filter.UserId.Value);
        }
        if (!string.IsNullOrEmpty(filter.FullName))
        {
            Query.Where(ua => ua.FullName == filter.FullName);
        }
        if (!string.IsNullOrEmpty(filter.City))
        {
            Query.Where(ua => ua.City == filter.City);
        }
        if (filter.CreatedAfter.HasValue)
        {
            Query.Where(ua => ua.CreatedAt >= filter.CreatedAfter.Value);
        }
        if (filter.CreatedBefore.HasValue)
        {
            Query.Where(ua => ua.CreatedAt <= filter.CreatedBefore.Value);
        }
        if (!string.IsNullOrEmpty(filter.Search))
        {
            Query.Where(ua =>
                ua.FullName.Contains(filter.Search) ||
                ua.Phone.Contains(filter.Search) ||
                ua.AddressLine.Contains(filter.Search) ||
                ua.Ward.Contains(filter.Search) ||
                ua.District.Contains(filter.Search) ||
                ua.City.Contains(filter.Search));
        }

        if (!string.IsNullOrEmpty(filter.SortBy))
        {
            switch (filter.SortBy.ToLower())
            {
                case "fullname":
                    if (filter.IsDescending) Query.OrderByDescending(x => x.FullName);
                    else Query.OrderBy(x => x.FullName);
                    break;
                case "city":
                    if (filter.IsDescending) Query.OrderByDescending(x => x.City);
                    else Query.OrderBy(x => x.City);
                    break;
                case "createdat":
                    if (filter.IsDescending) Query.OrderByDescending(x => x.CreatedAt);
                    else Query.OrderBy(x => x.CreatedAt);
                    break;
                default:
                    if (filter.IsDescending) Query.OrderByDescending(x => x.CreatedAt);
                    else Query.OrderBy(x => x.CreatedAt);
                    break;
            }
        }

        if (isPaging)
        {
            Query.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize);
        }
    }
}
