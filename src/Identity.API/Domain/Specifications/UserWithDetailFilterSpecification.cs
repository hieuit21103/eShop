namespace Identity.API.Domain.Specifications;

public class UserWithDetailFilterSpecification : Specification<ApplicationUser>
{
    public UserWithDetailFilterSpecification(ApplicationUserFilterParams filter, bool isPaging = false)
    {
        Query.Include(au => au.Profile);
        Query.Include(au => au.UserRoles).ThenInclude(ur => ur.Role);
        if (!string.IsNullOrEmpty(filter.RoleName))
        {
            Query.Where(au => au.UserRoles.Any(r => r.Role.Name == filter.RoleName));
        }

        if (filter.EmailConfirmed.HasValue)
        {
            Query.Where(au => au.EmailConfirmed == filter.EmailConfirmed.Value);
        }

        if (filter.PhoneNumberConfirmed.HasValue)
        {
            Query.Where(au => au.PhoneNumberConfirmed == filter.PhoneNumberConfirmed.Value);
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
                ua.UserName.Contains(filter.Search) ||
                ua.Email.Contains(filter.Search) ||
                ua.PhoneNumber.Contains(filter.Search) ||
                ua.Profile.FullName.Contains(filter.Search));
        }

        if (!string.IsNullOrEmpty(filter.SortBy))
        {
            switch (filter.SortBy.ToLower())
            {
                case "fullname":
                    if (filter.IsDescending) Query.OrderByDescending(x => x.Profile.FullName);
                    else Query.OrderBy(x => x.Profile.FullName);
                    break;
                case "username":
                    if (filter.IsDescending) Query.OrderByDescending(x => x.UserName);
                    else Query.OrderBy(x => x.UserName);
                    break;
                case "email":
                    if (filter.IsDescending) Query.OrderByDescending(x => x.Email);
                    else Query.OrderBy(x => x.Email);
                    break;
                case "phonenumber":
                    if (filter.IsDescending) Query.OrderByDescending(x => x.PhoneNumber);
                    else Query.OrderBy(x => x.PhoneNumber);
                    break;
                case "rolename":
                    if (filter.IsDescending) Query.OrderByDescending(x => x.UserRoles.FirstOrDefault()!.Role.Name);
                    else Query.OrderBy(x => x.UserRoles.FirstOrDefault()!.Role.Name);
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