namespace Identity.API.Seeders
{
    public static class SeedUserRole
    {
        public static async Task SeedUserRolesAsync(this IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            
            var admin = await userManager.FindByNameAsync("Admin");
            var user = await userManager.FindByNameAsync("User");
            
            if(admin == null || user == null)
            {
                return;
            }

            if(await userManager.IsInRoleAsync(admin, "Admin") && await userManager.IsInRoleAsync(user, "User"))
            {
                return;
            }
    
            await userManager.AddToRoleAsync(admin, "Admin");
            await userManager.AddToRoleAsync(user, "User");
        }
    }
}

