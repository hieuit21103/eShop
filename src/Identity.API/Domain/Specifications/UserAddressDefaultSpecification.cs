using Ardalis.Specification;

namespace Identity.API.Domain.Specifications;

public class UserAddressDefaultSpecification : Specification<UserAddress>
{
    public UserAddressDefaultSpecification(Guid userId)
    {
        Query.Where(ua => ua.UserId == userId && ua.IsDefault);
    }
}
