using Ardalis.Specification;

namespace Identity.API.Domain.Specifications;

public class UserAddressByUserIdSpecification : Specification<UserAddress>
{
    public UserAddressByUserIdSpecification(Guid userId)
    {
        Query.Where(ua => ua.UserId == userId);
    }
}
