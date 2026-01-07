using Google.Apis.Auth;
using Identity.API.Application.DTOs.Auth;
using Identity.API.Domain.Specifications;
using Identity.API.Domain.Interfaces;
using Shared.Events;
using MassTransit;

namespace Identity.API.Application.Services;

public class AuthService : IAuthService
{
    private readonly ILogger<AuthService> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtService _jwtService;
    private readonly IRepository<RefreshToken> _refreshTokenRepository;
    private readonly IConfiguration _configuration;
    private readonly IPublishEndpoint _publishEndpoint;

    public AuthService(
        ILogger<AuthService> logger,
        UserManager<ApplicationUser> userManager,
        IJwtService jwtService,
        IRepository<RefreshToken> refreshTokenRepository,
        IConfiguration configuration,
        IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _userManager = userManager;
        _jwtService = jwtService;
        _refreshTokenRepository = refreshTokenRepository;
        _configuration = configuration;
        _publishEndpoint = publishEndpoint;
    }

    // Authentication
    public async Task<IdentityResult> RegisterUserAsync(RegisterRequest request)
    {
        var user = new ApplicationUser
        {
            UserName = request.Username,
            Email = request.Email
        };

        var password = request.Password;

        var existingUser = await _userManager.FindByNameAsync(request.Username);
        if (existingUser != null)
        {
            throw new InvalidOperationException("Username already exists.");
        }

        var result = await _userManager.CreateAsync(user, password);
        if (result.Succeeded)
        {
            var token = Uri.EscapeDataString(await _userManager.GenerateEmailConfirmationTokenAsync(user));
            var baseUrl = _configuration["ClientApp:BaseUrl"] ?? "";
            var confirmEmailPath = _configuration["ClientApp:ConfirmEmailPath"] ?? "";
            var url = baseUrl + confirmEmailPath;
            var confirmLink = url + "?email=" + user.Email + "&token=" + token;

            await _publishEndpoint.Publish(new UserRegisteredEvent(user.Id, user.Email!, user.UserName!, confirmLink));
            _logger.LogInformation("User registered successfully.");
            await _userManager.AddToRoleAsync(user, "User");
            return result;
        }
        else
        {
            throw new InvalidOperationException("User registration failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var existingUser = await _userManager.FindByNameAsync(request.Username);
        if (existingUser == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        if (!await _userManager.IsEmailConfirmedAsync(existingUser))
        {
            throw new InvalidOperationException("Email not confirmed.");
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(existingUser, request.Password);
        if (!isPasswordValid)
        {
            throw new InvalidOperationException("Invalid password.");
        }

        return await MapToResponseAsync(existingUser);
    }

    public async Task<AuthResponse> LoginWithGoogleAsync(GoogleLoginRequest request)
    {
        GoogleJsonWebSignature.Payload payload;
        try
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string>() { _configuration["Google:ClientId"] ?? "" }
            };
            payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken, settings);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Google token validation failed.");
            throw new InvalidOperationException("Invalid Google token.");
        }

        var user = await _userManager.FindByEmailAsync(payload.Email);
        if (user == null)
        {
            user = new ApplicationUser
            {
                UserName = payload.Email,
                Email = payload.Email,
                EmailConfirmed = true
            };
            
            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException("Failed to create user from Google login.");
            }
            await _publishEndpoint.Publish(new UserRegisteredEvent(user.Id, user.Email!, user.UserName!, string.Empty));
            await _userManager.AddToRoleAsync(user, "User");
        }

        return await MapToResponseAsync(user);
    }

    public async Task<AuthResponse> RefreshTokenAsync(string token)
    {
        var spec = new RefreshTokenByTokenSpecification(token);
        var refreshToken = await _refreshTokenRepository.FirstOrDefaultAsync(spec);

        if (refreshToken == null)
        {
            throw new KeyNotFoundException("Refresh token not found.");
        }

        if (refreshToken.RevokedAt != null)
        {
            // Detect refresh token reuse
            _logger.LogWarning("Reuse of revoked refresh token detected. Revoking all tokens for user {UserId}", refreshToken.UserId);
            
            var allUserTokensSpec = new RefreshTokenByUserIdSpecification(refreshToken.UserId);
            var allUserTokens = await _refreshTokenRepository.ListAsync(allUserTokensSpec);
            
            foreach (var tokenToRevoke in allUserTokens)
            {
                tokenToRevoke.RevokedAt = DateTime.UtcNow;
            }
            await _refreshTokenRepository.UpdateRangeAsync(allUserTokens);

            throw new InvalidOperationException("Refresh token is revoked.");
        }

        if (refreshToken.ExpiresAt < DateTime.UtcNow)
        {
            throw new InvalidOperationException("Refresh token has expired.");
        }

        var user = refreshToken.User;
        if (user == null)
        {
            throw new KeyNotFoundException("User associated with refresh token not found.");
        }

        refreshToken.RevokedAt = DateTime.UtcNow;
        await _refreshTokenRepository.UpdateAsync(refreshToken);
        await _refreshTokenRepository.SaveChangesAsync();

        return await MapToResponseAsync(user);
    }

    public async Task SignOutUserAsync(string userId)
    {

        var spec = new RefreshTokenByUserIdSpecification(Guid.Parse(userId));
        var refreshToken = await _refreshTokenRepository.FirstOrDefaultAsync(spec) ?? throw new KeyNotFoundException("Refresh token not found.");
        if (refreshToken != null)
        {
            refreshToken.RevokedAt = DateTime.UtcNow;
            await _refreshTokenRepository.UpdateAsync(refreshToken);
            await _refreshTokenRepository.SaveChangesAsync();
        }
    }

    // Email and Password Management
    public async Task ResendConfirmationEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) throw new KeyNotFoundException("User not found.");

        if (user.EmailConfirmed) throw new InvalidOperationException("Email already confirmed.");

        var token = Uri.EscapeDataString(await _userManager.GenerateEmailConfirmationTokenAsync(user));
        var baseUrl = _configuration["ClientApp:BaseUrl"] ?? "";
        var confirmEmailPath = _configuration["ClientApp:ConfirmEmailPath"] ?? "";
        var url = baseUrl + confirmEmailPath;
        var confirmLink = url + "?email=" + user.Email + "&token=" + token;
        
        await _publishEndpoint.Publish(new UserRegisteredEvent(user.Id, user.Email!, user.UserName!, confirmLink));
    }

    public async Task<IdentityResult> ConfirmEmailAsync(string email, string token)
    {
        token = Uri.UnescapeDataString(token);
        var user = await _userManager.FindByEmailAsync(email) ?? throw new Exception("User not found.");
        var result = await _userManager.ConfirmEmailAsync(user, token);
        return result;
    }

    public async Task<IdentityResult> ChangePasswordAsync(string userId, ChangePasswordRequest request)
    {
        var user = await _userManager.FindByIdAsync(userId) ?? throw new Exception("User not found.");
        var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        return result;
    }

    public async Task RequestPasswordResetAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email) ?? throw new Exception("User not found.");
        var token = Uri.EscapeDataString(await _userManager.GeneratePasswordResetTokenAsync(user));
        var baseUrl = _configuration["ClientApp:BaseUrl"] ?? "";
        var resetPasswordPath = _configuration["ClientApp:ResetPasswordPath"] ?? "";
        var url = baseUrl + resetPasswordPath;
        var resetLink = url + "?email=" + user.Email + "&token=" + token;
        
        await _publishEndpoint.Publish(new UserPasswordResetRequestedEvent(user.Email!, resetLink));
    }

    public async Task<IdentityResult> ResetPasswordAsync(string email, string token, string newPassword)
    {
        token = Uri.UnescapeDataString(token);
        var user = await _userManager.FindByEmailAsync(email) ?? throw new Exception("User not found.");
        var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
        return result;
    }

    private async Task<AuthResponse> MapToResponseAsync(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        var accessToken = _jwtService.GenerateToken(user, roles);
        var refreshToken = _jwtService.GenerateRefreshToken();

        refreshToken.UserId = user.Id;
        await _refreshTokenRepository.AddAsync(refreshToken);
        await _refreshTokenRepository.SaveChangesAsync();

        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token,
            ExpiresAt = refreshToken.ExpiresAt
        };
    }
}
