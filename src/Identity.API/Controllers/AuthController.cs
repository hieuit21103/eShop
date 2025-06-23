using Identity.API.Models.DTOs;
using Identity.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly JwtService _jwtService;

        public AuthController(AuthService authService, JwtService jwtService)
        {
            _authService = authService;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (registerDto == null)
            {
                return BadRequest("Invalid registration data.");
            }

            var result = await _authService.RegisterUserAsync(registerDto);
            if (result.Succeeded)
            {
                return Ok(new { Message = "User registered successfully." });
            }

            return BadRequest(result.Errors);
        }

        [HttpGet("confirm-email/{userId}/{token}")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var result = await _authService.ConfirmEmailAsync(userId, token);
            if (result.Succeeded)
            {
                return Ok(new { Message = "Email confirmed successfully." });
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> SignIn([FromBody] LoginDto loginDto)
        {
            if (loginDto == null)
            {
                return BadRequest("Invalid login data.");
            }

            var result = await _authService.SignInUserAsync(loginDto);
            if (result.Succeeded)
            {
                var user = await _authService.GetUserByUsernameAsync(loginDto.Username);
                var token = _jwtService.GenerateToken(user);
                return Ok(new { Token = token });
            }
            return Unauthorized("Invalid username or password.");
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authService.SignOutUserAsync();
            return Ok(new { Message = "User logged out successfully." });
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await _authService.GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound("User not found.");
            }

            return Ok(new
            {
                user.Id,
                user.UserName,
                user.Email,
                FullName = user.Profile?.FullName,
                AvatarUrl = user.Profile?.AvatarUrl
            });
        }
    }
}