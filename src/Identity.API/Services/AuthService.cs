using Identity.API.Data;
using Identity.API.Models;
using Identity.API.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using DotNetEnv;

namespace Identity.API.Services
{

    public class AuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly EmailService _emailService;
        private readonly ApplicationDbContext _context;

        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, EmailService emailService, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _emailService = emailService;
            Env.Load();
            Env.TraversePath().Load();
        }

        public async Task<IdentityResult> RegisterUserAsync(RegisterDto registerDto)
        {
            var user = new ApplicationUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Email
            };

            var password = registerDto.Password;

            var existingUser = await _userManager.FindByNameAsync(registerDto.Username);
            if (existingUser != null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User already exists." });
            }

            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                var apiUrl = Environment.GetEnvironmentVariable("API_URL");
                var confirmationLink = apiUrl + "/api/auth/confirm-email/" + user.Id + "/" + Uri.EscapeDataString(await _userManager.GenerateEmailConfirmationTokenAsync(user));
                await _emailService.SendConfirmationEmailAsync(user.Email, confirmationLink);
                await _userManager.AddToRoleAsync(user, "User");
                return result;
            }
            else
            {
                return IdentityResult.Failed(result.Errors.ToArray());
            }
        }

        public async Task<SignInResult> SignInUserAsync(LoginDto loginDto)
        {
            var existingUser = await _userManager.FindByNameAsync(loginDto.Username);
            if (existingUser == null)
            {
                return SignInResult.Failed;
            }

            if (!await _userManager.IsEmailConfirmedAsync(existingUser))
            {
                return SignInResult.NotAllowed;
            }

            var result = await _signInManager.PasswordSignInAsync(
                existingUser.UserName,
                loginDto.Password,
                isPersistent: false,
                lockoutOnFailure: false
            );
            return result;
        }

        public async Task SignOutUserAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<ApplicationUser> GetUserByUsernameAsync(string username)
        {
            return await _userManager.FindByNameAsync(username) ?? throw new Exception("User not found.");
        }

        public async Task<ApplicationUser> GetCurrentUserAsync()
        {
            var user = _signInManager.Context.User;
            if (user == null || !user.Identity.IsAuthenticated)
            {
                return null;
            }

            var userId = _userManager.GetUserId(user);
            return await _userManager.FindByIdAsync(userId) ?? throw new Exception("User not found.");
        }

        public async Task<IdentityResult> ConfirmEmailAsync(string userId, string token)
        {
            token = Uri.UnescapeDataString(token);
            var user = await _userManager.FindByIdAsync(userId) ?? throw new Exception("User not found.");
            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result;
        }

        public async Task SendPasswordResetEmailAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId) ?? throw new Exception("User not found.");
            var token = Uri.EscapeDataString(await _userManager.GeneratePasswordResetTokenAsync(user));
            var apiUrl = Environment.GetEnvironmentVariable("API_URL");
            var resetLink = apiUrl + "/api/auth/reset-password/" + user.Id + "/" + token;
            await _emailService.SendPasswordResetEmailAsync(user.Email, resetLink);
        }

        public async Task<IdentityResult> ResetPasswordAsync(string userId, string token, string newPassword)
        {
            token = Uri.UnescapeDataString(token);
            var user = await _userManager.FindByIdAsync(userId) ?? throw new Exception("User not found.");
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            return result;
        }
    }
}