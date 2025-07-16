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
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .SelectMany(x => x.Value.Errors.Select(e => new
                    {
                        code = e.ErrorMessage.Split(':')[0],
                        description = e.ErrorMessage.Split(':')[1]
                    }))
                    .ToList();
                return BadRequest(errors);
            }
            if (registerDto == null)
            {
                return BadRequest("Invalid registration data.");
            }

            var result = await _authService.RegisterUserAsync(registerDto);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToArray();
                return BadRequest(errors);
            }
            return Ok(new { Message = "User registered successfully." });
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

            var user = await _authService.SignInUserAsync(loginDto);
            if (user == null)
            {
                return Unauthorized("Invalid username or password.");
            }
            
            var token = await _jwtService.GenerateToken(user);
            Response.Cookies.Append("JWT", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });
            return Ok();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authService.SignOutUserAsync();
            Response.Cookies.Delete("JWT");
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
            });
        }

        [HttpGet("forgot-password/{email}")]
        public async Task<IActionResult> ForgotPassword([FromRoute] string email)
        {
            var userId = await _authService.GetUserIdByEmailAsync(email);
            await _authService.SendPasswordResetEmailAsync(userId);
            return Ok(new { Message = "Password reset email sent successfully." });
        }

        [HttpPut("reset-password/{userId}/{token}")]
        public async Task<IActionResult> ResetPassword([FromRoute] string userId, [FromRoute] string token, [FromBody] ResetPasswordDto resetPasswordDto)
        {
            var user = await _authService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            if (resetPasswordDto.Password != resetPasswordDto.ConfirmPassword)
            {
                return BadRequest("Passwords do not match.");
            }
            var newPassword = resetPasswordDto.Password;
            var result = await _authService.ResetPasswordAsync(userId, token, newPassword);
            if (result.Succeeded)
            {
                return Ok(new { Message = "Password reset successfully." });
            }
            return BadRequest(result.Errors);
        }
    }
}