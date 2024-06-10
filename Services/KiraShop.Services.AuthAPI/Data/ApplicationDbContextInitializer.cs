using KiraShop.Services.AuthAPI.Enums;
using KiraShop.Services.AuthAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KiraShop.Services.AuthAPI.Data
{
    public static class InitializerExtensions
    {
        public static async Task InitializeDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();

            await initialiser.InitializeAsync();

            await initialiser.SeedAsync();
        }
    }

    public class ApplicationDbContextInitializer
    {
        private readonly ILogger<ApplicationDbContextInitializer> _logger;
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public ApplicationDbContextInitializer(
            ILogger<ApplicationDbContextInitializer> logger,
            AppDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task InitializeAsync()
        {
            try
            {
                await _context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while initialising the database.");
                throw;
            }
        }

        public async Task SeedAsync()
        {
            try
            {
                await TrySeedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }

        public async Task TrySeedAsync()
        {
            // Seed roles
            if (_roleManager.Roles.All(r => r.Name != Roles.Admin.ToString()))
            {
                await _roleManager.CreateAsync(new ApplicationRole() { Name = Roles.Admin.ToString() });
            }

            if (_roleManager.Roles.All(r => r.Name != Roles.User.ToString()))
            {
                await _roleManager.CreateAsync(new ApplicationRole() { Name = Roles.User.ToString() });
            }

            // Seed admin user
            // Default admin
            var administrator = new ApplicationUser { UserName = "administrator@gmail.com", Email = "administrator@gmail.com", EmailConfirmed = true };

            if (_userManager.Users.All(u => u.Email != administrator.Email))
            {
                await _userManager.CreateAsync(administrator, "Administrator1!");
                var administratorRole = new IdentityRole(Roles.Admin.ToString());
                if (!string.IsNullOrWhiteSpace(administratorRole.Name))
                {
                    await _userManager.AddToRolesAsync(administrator, [administratorRole.Name]);
                }
            }
        }
    }
}
