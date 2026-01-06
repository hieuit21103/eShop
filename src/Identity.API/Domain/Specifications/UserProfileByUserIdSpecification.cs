using Ardalis.Specification;

namespace Identity.API.Domain.Specifications;

public class UserProfileByUserIdSpecification : Specification<UserProfile>
{
    public UserProfileByUserIdSpecification(Guid userId)
    {
        Query.Where(up => up.UserId == userId);
    }
}
