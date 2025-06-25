using Identity.API.Data;
using Identity.API.Models;
using Identity.API.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using DotNetEnv;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
                await SendConfirmationEmailAsync(user.Id);
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

        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId) ?? throw new Exception("User not found.");
        }

        public async Task<string> GetUserIdByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email) ?? throw new Exception("User not found.");
            return user.Id;
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

        public async Task SendConfirmationEmailAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId) ?? throw new Exception("User not found.");
            var token = Uri.EscapeDataString(await _userManager.GenerateEmailConfirmationTokenAsync(user));
            var url = Environment.GetEnvironmentVariable("URL");
            var confirmLink = url + "/confirm-email?userId=" + user.Id + "&token=" + token;
            await _emailService.SendConfirmationEmailAsync(user.Email, confirmLink);
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
            var url = Environment.GetEnvironmentVariable("URL");
            var resetLink = url + "/reset-password?userId=" + user.Id + "&token=" + token;
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
