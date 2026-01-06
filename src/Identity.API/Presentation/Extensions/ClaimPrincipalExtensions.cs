namespace Identity.API.Presentation.Controllers.Extensions;

public static class ClaimPrincipalExtensions
{
    public static Guid? GetCurrentUserId(this ClaimsPrincipal user)
    {
        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null) return null;
        if (!Guid.TryParse(userIdClaim.Value, out var userId)) return null;
        return userId;
    }

    public static string? GetCurrentUserEmail(this ClaimsPrincipal user)
    {
        return user.FindFirst(ClaimTypes.Email)?.Value;
    }

    public static bool IsInRole(this ClaimsPrincipal user, string role)
    {
        return user.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == role);
    }
}