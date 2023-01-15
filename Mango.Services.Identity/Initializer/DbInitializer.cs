using System.Security.Claims;
using IdentityModel;
using Mango.Services.Identity.DbContext;
using Mango.Services.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Mango.Services.Identity.Initializer;

public class DbInitializer : IDbInitializer
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public DbInitializer(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }


    public void Initialize()
    {
        if (_roleManager.FindByNameAsync("Admin").Result == null)
        {
            _roleManager.CreateAsync(new(SD.Admin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new(SD.Customer)).GetAwaiter().GetResult();
        }
        else
        {
            return;
        }

        ApplicationUser adminUser = new()
        {
            UserName = "admin1@gmail.com",
            Email = "admin1@gmail.com",
            EmailConfirmed = true,
            PhoneNumber = "1234567890",
            FirstName = "Ben",
            LastName = "Hunt"
        };

        _userManager.CreateAsync(adminUser, "Admin@123").GetAwaiter().GetResult();
        _userManager.AddToRoleAsync(adminUser, SD.Admin).GetAwaiter().GetResult();
        var temp1 = _userManager.AddClaimsAsync(adminUser, new Claim[]{
            new(JwtClaimTypes.Name, adminUser.FirstName+" "+adminUser.LastName),
            new(JwtClaimTypes.GivenName, adminUser.FirstName),
            new(JwtClaimTypes.FamilyName, adminUser.LastName),
            new(JwtClaimTypes.Role, SD.Admin),
            
        }).Result;

        ApplicationUser user = new()
        {
            UserName = "customer1@gmail.com",
            Email = "customer1@gmail.com",
            EmailConfirmed = true,
            PhoneNumber = "1234567890",
            FirstName = "Ben",
            LastName = "Hunt"
        };

        _userManager.CreateAsync(user, "Customer@123").GetAwaiter().GetResult();
        _userManager.AddToRoleAsync(user, SD.Customer).GetAwaiter().GetResult();
        var temp2 = _userManager.AddClaimsAsync(user, new Claim[]{
            new(JwtClaimTypes.Name, user.FirstName+" "+user.LastName),
            new(JwtClaimTypes.GivenName, user.FirstName),
            new(JwtClaimTypes.FamilyName, user.LastName),
            new(JwtClaimTypes.Role, SD.Customer),

        }).Result;
    }
}