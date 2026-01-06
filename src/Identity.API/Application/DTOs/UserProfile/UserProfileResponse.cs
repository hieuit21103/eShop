namespace Identity.API.Application.DTOs.UserProfile;

public class UserProfileResponse
{
    public Guid Id { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public DateTime? DateOfBirth { get; set; }
    public Gender? Gender { get; set; }
    public Guid AvatarId { get; set; }
}
