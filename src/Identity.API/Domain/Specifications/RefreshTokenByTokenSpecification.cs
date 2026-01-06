using Ardalis.Specification;
using Identity.API.Domain.Entities;

namespace Identity.API.Domain.Specifications;

public class RefreshTokenByTokenSpecification : Specification<RefreshToken>
{
    public RefreshTokenByTokenSpecification(string token)
    {
        Query.Where(rt => rt.Token == token)
             .Include(rt => rt.User);
    }
}
