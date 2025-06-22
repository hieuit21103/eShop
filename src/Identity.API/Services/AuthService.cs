using Identity.API.Data;
using Identity.API.Models;
using Identity.API.Models.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Identity.API.Services
{

    public class AuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
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
    }
}