using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer
{
    public class Config
    {

        // Resources and Api want to protect
        public static IEnumerable<IdentityResource> IdentityResources =>
            //Adding support for OpenID Connect Identity Scopes
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };


        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("dataEventRecordsApi","Data Event Records API",new List<string> { "role", "admin", "user", "dataEventRecords", "dataEventRecords.admin", "dataEventRecords.user" })
                {
                    ApiSecrets =
                    {
                        new Secret("dataEventRecordsSecret".Sha256())
                    },
                    Scopes =
                    {
                        "dataEventRecordsScope",
                    }
                }
            };
        }




        public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new ApiScope("api1", "My API"),
            new ApiScope
                {
                    Name = "dataEventRecordsScope",
                    DisplayName = "Scope for the dataEventRecords ApiResource",
                    UserClaims = { "role", "admin", "user", "dataEventRecords", "dataEventRecords.admin", "dataEventRecords.user" }
                }
        };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "client",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = { "api1" }
                },


                new Client
                {
                    ClientId = "ro.client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "api1",IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile }
                },

                new Client
                {
                    ClientId = "resourceownerclient",

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AccessTokenType = AccessTokenType.Jwt,
                    AccessTokenLifetime = 3600,
                    IdentityTokenLifetime = 3600,
                    UpdateAccessTokenClaimsOnRefresh = true,
                    SlidingRefreshTokenLifetime = 30,
                    AllowOfflineAccess = true,
                    RefreshTokenExpiration = TokenExpiration.Absolute,
                    RefreshTokenUsage = TokenUsage.OneTimeOnly,
                    AlwaysSendClientClaims = true,
                    Enabled = true,
                    ClientSecrets=  new List<Secret> { new Secret("dataEventRecordsSecret".Sha256()) },
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,                        
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "dataEventRecordsScope"
                    }
                }
            };

        internal static List<TestUser> GetUser()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "alice",
                    Claims = new List<Claim>
                    {
                        new Claim("email", "email@gamil.com"),
                        new Claim("sdt", "009976876")
                    },
                    Password = "password"
                   
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "bob",
                    Password = "password"
                }
            };
        }
    }
}
