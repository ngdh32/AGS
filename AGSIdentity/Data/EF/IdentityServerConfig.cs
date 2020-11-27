using System;
using System.Collections.Generic;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;

namespace AGSIdentity.Data.EF
{
    public class IdentityServerConfig
    {
        
        
        private const string AGSDocumentScopeConstant = "ags.document";
        private const string AGSFunctionClaimResouceConstant = "FunctionClaimResource";


        private IConfiguration _configuration { get; set; }

        public IdentityServerConfig(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEnumerable<IdentityResource> GetIdentityResources()
        {
            // add roles and role-related claims to identity resource
            var roleResource = new IdentityResource(
                name: AGSFunctionClaimResouceConstant,
                displayName: "User's roles and role-related claims",
                claimTypes: new[] { CommonConstant.FunctionClaimTypeConstant, JwtClaimTypes.Role });

            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                roleResource
            };
        }

        public IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>
            {
                new ApiResource{
                    Name = CommonConstant.AGSIdentityScopeConstant,
                    Scopes =
                    {
                        new Scope()
                        {
                            Name = CommonConstant.AGSIdentityScopeConstant,
                            DisplayName = "Full access to ags.identity",
                            UserClaims = {
                                JwtClaimTypes.Id
                               ,JwtClaimTypes.Subject
                               ,JwtClaimTypes.Email
                               ,JwtClaimTypes.Profile
                               ,AGSFunctionClaimResouceConstant
                            }

                        }
                    }
                }
                ,
                new ApiResource{
                    Name = AGSDocumentScopeConstant,

                    Scopes =
                    {
                        new Scope()
                        {
                            Name = AGSDocumentScopeConstant,
                            DisplayName = "Full access to ags.document",
                            UserClaims = {
                                JwtClaimTypes.Name
                               ,JwtClaimTypes.Email
                               ,JwtClaimTypes.Profile
                               ,AGSFunctionClaimResouceConstant
                            }

                        }
                    }
                }
            };
        }

        public IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = CommonConstant.AGSClientIdConstant,

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                    RequireConsent = false,
                    RedirectUris = {
                        _configuration["ags_web_url"] + "auth/signin-oidc"
                    },
                    PostLogoutRedirectUris =
                    {
                        _configuration["ags_web_url"] +  "auth/signout-callback-oidc"
                    },

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret(_configuration["ags_web_secret"].Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = {
                        AGSDocumentScopeConstant
                        ,CommonConstant.AGSIdentityScopeConstant
                        ,IdentityServerConstants.StandardScopes.OpenId
                        ,IdentityServerConstants.StandardScopes.Profile
                        ,IdentityServerConstants.StandardScopes.Email
                        ,AGSFunctionClaimResouceConstant
                    },
                    
                    AllowAccessTokensViaBrowser = true,
                    AlwaysIncludeUserClaimsInIdToken = true
                }
            };
        }
    }
}
