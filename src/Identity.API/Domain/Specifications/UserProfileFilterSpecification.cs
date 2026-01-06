using Ardalis.Specification;

namespace Identity.API.Domain.Specifications;

public class UserProfileFilterSpecification : Specification<UserProfile>
{
    public UserProfileFilterSpecification(UserProfileFilterParams filter, bool isPaging = false)
    {
        if (!string.IsNullOrEmpty(filter.Search))
        {
            Query.Where(up => up.FullName.Contains(filter.Search));
        }

        if (!string.IsNullOrEmpty(filter.FullName))
        {
            Query.Where(up => up.FullName.Contains(filter.FullName));
        }

        if (filter.CreatedAfter.HasValue)
        {
            Query.Where(up => up.CreatedAt >= filter.CreatedAfter.Value);
        }

        if (filter.CreatedBefore.HasValue)
        {
            Query.Where(up => up.CreatedAt <= filter.CreatedBefore.Value);
        }

        if (!string.IsNullOrEmpty(filter.SortBy))
        {
            switch (filter.SortBy.ToLower())
            {
                case "fullname":
                    if (filter.IsDescending) Query.OrderByDescending(x => x.FullName);
                    else Query.OrderBy(x => x.FullName);
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
