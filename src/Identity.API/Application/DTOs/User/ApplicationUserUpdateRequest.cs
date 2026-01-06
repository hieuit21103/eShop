namespace Identity.API.Application.DTOs.User;

public class ApplicationUserUpdateRequest
{
    public string Id { get; set; } = null!;
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}   