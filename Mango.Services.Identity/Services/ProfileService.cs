using System.Security.Claims;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using Mango.Services.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Mango.Services.Identity.Services;

public class ProfileService : IProfileService
{
    private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimPrincipalFactory;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ProfileService(
        IUserClaimsPrincipalFactory<ApplicationUser> userClaimPrincipalFactory,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager
        )
    {
        _userClaimPrincipalFactory = userClaimPrincipalFactory;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        string sub = context.Subject.GetSubjectId();
        ApplicationUser user = await _userManager.FindByIdAsync(sub);
        ClaimsPrincipal userClaims = await _userClaimPrincipalFactory.CreateAsync(user);

        var listOfClaims = userClaims.Claims.ToList();
        listOfClaims = listOfClaims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();

        if (_userManager.SupportsUserRole)
        {
            IList<string> roles = await _userManager.GetRolesAsync(user);
            foreach (string roleName in roles)
            {
                listOfClaims.Add(new Claim(JwtClaimTypes.Role, roleName));
                listOfClaims.Add(new Claim(JwtClaimTypes.FamilyName, user.LastName));
                listOfClaims.Add(new Claim(JwtClaimTypes.GivenName, user.FirstName));
                IdentityRole role = await _roleManager.FindByNameAsync(roleName);
                if (role != null)
                {
                    IList<Claim> roleClaims = await _roleManager.GetClaimsAsync(role);
                    foreach (Claim roleClaim in roleClaims)
                    {
                        if (!listOfClaims.Contains(roleClaim))
                        {
                            listOfClaims.Add(roleClaim);
                        }
                    }

                }
            }
        }

        context.IssuedClaims = listOfClaims;
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        string sub = context.Subject.GetSubjectId();
        ApplicationUser user = await _userManager.FindByIdAsync(sub);
        context.IsActive = user != null;

    }
}