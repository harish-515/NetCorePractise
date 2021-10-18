using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer
{
    public static class Configurations
    {
        //public static IEnumerable<ApiScope> GetApiScopes => new List<ApiScope> { new ApiScope("ApiOne") }; 
        public static IEnumerable<ApiResource> GetResourceScopes => new List<ApiResource> {
                new ApiResource("ApiOne"),
                new ApiResource("ApiTwo",new string[] { "some.api.claim"})  // eventhough this claim is set for apitwo when creating an access token
                                                                            // the claims of all the relavent scopes are aggregated and will be accessable
                                                                            // in all the resources 
        };

        public static IEnumerable<IdentityResource> GetUserScopes => new List<IdentityResource> { 
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),

            // custom scopes should be added here to make them visible
            new IdentityResource
            {
                Name = "custom.scopes",
                UserClaims =
                {
                    "some.custom.claim"
                }
            }
        };
        public static IEnumerable<Client> GetClients => new List<Client> { 
            new Client()
            { 
                ClientId = "client_id",
                ClientSecrets = { new Secret("client_secret".ToSha256())},
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = { "ApiOne" }
            },
            new Client()
            {
                ClientId = "client_id_mvc",
                ClientSecrets = { new Secret("client_secret_mvc".ToSha256())},
                AllowedGrantTypes = GrantTypes.Code,
                RedirectUris = { "https://localhost:44321/signin-oidc" },
                AllowedScopes = { 
                    "ApiOne", 
                    "ApiTwo",
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "custom.scopes"  // we need to add sustom scope then to the list of scopes that this client can access.
                },

                // puts all the claims into the ID token
                //AlwaysIncludeUserClaimsInIdToken = true,
                
                // after login the identity server redirects to a consent page ... to make user aware of the claims 
                // that the application can access .. but if we choose not to show that screen we need to set to false
                RequireConsent = false
            }
        };

    }
}
