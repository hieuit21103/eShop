namespace Identity.API.Domain.Specifications;

public class RefreshTokenByUserIdSpecification : Specification<RefreshToken>
{
    public RefreshTokenByUserIdSpecification(Guid userId)
    {
        Query.Where(rt => rt.UserId == userId && rt.RevokedAt == null && rt.ExpiresAt > DateTime.UtcNow);
    }
}