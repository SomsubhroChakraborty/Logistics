using Logistics.Web.Data;
using Logistics.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Logistics.Web.Utilities;

public class DbInitializer : IDbInitializer
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public DbInitializer(
        ApplicationDbContext db,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _db = db;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public void Initialize()
    {
        try
        {
            // Apply any pending migrations
            if (_db.Database.GetPendingMigrations().Any())
            {
                _db.Database.Migrate();
            }

            // If roles already exist, do nothing
            if (_roleManager.RoleExistsAsync(WebsiteRoles.Admin).GetAwaiter().GetResult())
            {
                return;
            }

            // Create roles
            _roleManager.CreateAsync(new IdentityRole(WebsiteRoles.Admin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(WebsiteRoles.Manager)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(WebsiteRoles.Employee)).GetAwaiter().GetResult();

            // Create first admin user
            var adminUser = new ApplicationUser
            {
                UserName = "admin@logistics.com",
                Email = "admin@logistics.com",
                EmailConfirmed = true
            };

            _userManager.CreateAsync(adminUser, "Admin@123").GetAwaiter().GetResult();

            // Assign Admin role
            _userManager.AddToRoleAsync(adminUser, WebsiteRoles.Admin).GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            throw new Exception("Error occurred while initializing the database.", ex);
        }
    }
}