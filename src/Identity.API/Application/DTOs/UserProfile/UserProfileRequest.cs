using Identity.API.Domain.Enums;

namespace Identity.API.Application.DTOs.UserProfile;

public class UserProfileRequest
{
    public Guid? UserId { get; set; }
    public required string FullName { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public Gender? Gender { get; set; }
    public IFormFile? Avatar { get; set; }
}
