using Identity.API.Application.DTOs;
using Identity.API.Application.DTOs.Auth;
using Identity.API.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var result = await _authService.RegisterUserAsync(request);
        if (!result.Succeeded)
        {
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "Registration failed",
                Errors = result.Errors.Select(e => e.Description).ToList()
            });
        }
        return Ok(new ApiResponse
        {
            Success = true,
            Message = "User registered successfully"
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var response = await _authService.LoginAsync(request);

        if (response == null)
        {
            return Unauthorized(new ApiResponse
            {
                Success = false,
                Message = "Invalid username or password"
            });
        }

        var refreshToken = response.RefreshToken;
        HttpContext.Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddDays(7)
        });
        return Ok(new ApiResponse<AuthResponse>
        {
            Success = true,
            Message = "Login successful",
            Data = response
        });
    }

    [HttpPost("google-login")]
    public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest request)
    {
        var response = await _authService.LoginWithGoogleAsync(request);
        return Ok(new ApiResponse<AuthResponse>
        {
            Success = true,
            Message = "Login with Google successful",
            Data = response
        });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken()
    {
        var refreshToken = HttpContext.Request.Cookies["refreshToken"];
        if (refreshToken == null) return BadRequest("Refresh token is missing");
        var response = await _authService.RefreshTokenAsync(refreshToken);
        refreshToken = response.RefreshToken;
        HttpContext.Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddDays(7)
        });
        return Ok(new ApiResponse<AuthResponse>
        {
            Success = true,
            Message = "Token refreshed successfully",
            Data = response
        });
    }

    [HttpPost("signout/{userId}")]
    public async Task<IActionResult> SignOut(string userId)
    {
        await _authService.SignOutUserAsync(userId);
        return Ok(new ApiResponse
        {
            Success = true,
            Message = "User signed out successfully"
        });
    }

    [HttpPost("resend-confirmation-email")]
    public async Task<IActionResult> ResendConfirmationEmail([FromBody] string email)
    {
        await _authService.ResendConfirmationEmailAsync(email);
        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Confirmation email resent successfully"
        });
    }

    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string email, [FromQuery] string token)
    {
        var result = await _authService.ConfirmEmailAsync(email, token);
        if (!result.Succeeded)
        {
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "Email confirmation failed",
                Errors = result.Errors.Select(e => e.Description).ToList()
            });
        }
        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Email confirmed successfully"
        });
    }

    [HttpPost("change-password/{userId}")]
    public async Task<IActionResult> ChangePassword(string userId, [FromBody] ChangePasswordRequest request)
    {
        var result = await _authService.ChangePasswordAsync(userId, request);
        if (!result.Succeeded)
        {
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "Password change failed",
                Errors = result.Errors.Select(e => e.Description).ToList()
            });
        }
        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Password changed successfully"
        });
    }

    [HttpPost("request-password-reset")]
    public async Task<IActionResult> RequestPasswordReset([FromBody] string email)
    {
        await _authService.RequestPasswordResetAsync(email);
        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Password reset email sent successfully"
        });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromQuery] string userId, [FromQuery] string token, [FromBody] ResetPasswordRequest request)
    {
        if (request.Password != request.ConfirmPassword)
        {
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "Password and confirmation do not match"
            });
        }

        var result = await _authService.ResetPasswordAsync(userId, token, request.Password);
        if (!result.Succeeded)
        {
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "Password reset failed",
                Errors = result.Errors.Select(e => e.Description).ToList()
            });
        }
        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Password reset successfully"
        });
    }
}
