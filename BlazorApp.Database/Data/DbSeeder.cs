using BlazorApp.Models.Entities;
using Microsoft.AspNetCore.Identity;

public static class DbSeeder
{
    public static async Task SeedAdminUser(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        if (!await roleManager.RoleExistsAsync("User"))
        {
            await roleManager.CreateAsync(new IdentityRole("User"));
        }

        var adminUser = await userManager.FindByEmailAsync("admin@domain.com");
        if (adminUser == null)
        {
            var newAdmin = new User
            {
                UserName = "admin",
                Email = "admin@domain.com",
                FirstName = "Admin",
                Surname = "Admin"
            };

            var result = await userManager.CreateAsync(newAdmin, "Admin@1234");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(newAdmin, "Admin");
            }
        }
    }
}
