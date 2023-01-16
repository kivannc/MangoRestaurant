using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Mango.Services.Identity;

public static class SD
{
    public const string Admin = "Admin";
    public const string Customer = "Custumer";

    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Email(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new("mango", "Mango Server"),
            new(name: "read", displayName: "Read Access"),
            new(name: "write", displayName: "Write Access"),
            new(name: "delete", displayName: "Delete Access"),
        };

    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            new Client()
            {
                ClientId = "client",
                ClientSecrets = { new Secret("secret".Sha256()) },
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = { "read", "write", "profile" }
            },
            new Client()
            {
                ClientId = "mango",
                ClientSecrets = { new Secret("secret".Sha256()) },
                AllowedGrantTypes = GrantTypes.Code,
                RedirectUris = { "https://localhost:7174/signin-oidc", "https://localhost:44300/signin-oidc" },
                PostLogoutRedirectUris = { "https://localhost:7174/signout-callback-oidc", "https://localhost:44300/signout-callback-oidc" },
                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    "mango",
                },
            }
        };
}