using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Identity.API.Models;

namespace Identity.API.Seeders
{
    public static class SeedUser
    {
        public static async Task SeedUsersAsync(this IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var users = new[] { "Admin", "User" };
            foreach (var user in users)
            {
                if (await userManager.FindByNameAsync(user) == null)
                {
                    var result = await userManager.CreateAsync(
                        new ApplicationUser
                        {
                            UserName = user,
                            Email = user + "@gmail.com"
                        },
                        "Password@123");
                    if (!result.Succeeded)
                    {
                        Console.WriteLine(result.Errors);
                    }
                }
            }
        }
    }
}