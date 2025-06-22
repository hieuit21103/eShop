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
            var user = new ApplicationUser
            {
                UserName = loginDto.Username
            };

            var password = loginDto.Password;

            var existingUser = await _userManager.FindByNameAsync(loginDto.Username);
            if (existingUser == null)
            {
                return SignInResult.Failed;
            }

            var result = await _signInManager.PasswordSignInAsync(user, password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return SignInResult.Success;
            }
            else if (result.IsLockedOut)
            {
                return SignInResult.LockedOut;
            }
            else
            {
                return SignInResult.Failed;
            }
        }

        public async Task SignOutUserAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<ApplicationUser> GetUserByUsernameAsync(string username)
        {
            return await _userManager.FindByNameAsync(username) ?? throw new Exception("User not found.");
        }
    }
}