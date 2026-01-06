namespace Identity.API.Application.Interfaces;

public interface IAuthService
{
    // Authentication
    Task<IdentityResult> RegisterUserAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<AuthResponse> LoginWithGoogleAsync(GoogleLoginRequest request);
    Task<AuthResponse> RefreshTokenAsync(string token);
    Task SignOutUserAsync(string userId);

    // Email and Password Management
    Task ResendConfirmationEmailAsync(string email);
    Task<IdentityResult> ConfirmEmailAsync(string email, string token);
    Task<IdentityResult> ChangePasswordAsync(string userId, ChangePasswordRequest request);
    Task RequestPasswordResetAsync(string email);
    Task<IdentityResult> ResetPasswordAsync(string email, string token, string newPassword);
}
