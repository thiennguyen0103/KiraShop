using KiraShop.Services.AuthAPI.Enums;
using KiraShop.Services.AuthAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace KiraShop.Services.AuthAPI.Data
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new ApplicationRole() { Name = Roles.Admin.ToString(), NormalizedName = Roles.Admin.ToString() });
            await roleManager.CreateAsync(new ApplicationRole() { Name = Roles.User.ToString(), NormalizedName = Roles.User.ToString() });
        }
    }
}
