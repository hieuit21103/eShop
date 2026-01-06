namespace Identity.API.Application.DTOs.UserProfile;

public class UserProfileUpdateRequest
{
    public string? FullName { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public Gender? Gender { get; set; }
    public IFormFile? Avatar { get; set; }
}
