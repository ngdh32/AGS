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
        public IdentityServerConfig()
        {
            
        }

        public IEnumerable<IdentityResource> GetIdentityResources()
        {
            // add roles and role-related claims to identity resource
            var roleResource = new IdentityResource(
                name: AGSCommon.CommonConstant.AGSIdentityConstant.AGSFunctionScopeConstant,
                displayName: "User's roles and role-related claims",
                claimTypes: new[] { AGSCommon.CommonConstant.AGSIdentityConstant.FunctionClaimTypeConstant, JwtClaimTypes.Role });

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
                    Name = AGSCommon.CommonConstant.AGSIdentityConstant.AGSIdentityScopeConstant,
                    Scopes =
                    {
                        new Scope()
                        {
                            Name = AGSCommon.CommonConstant.AGSIdentityConstant.AGSIdentityScopeConstant,
                            DisplayName = "Full access to ags.identity",
                            UserClaims = {
                                JwtClaimTypes.Id
                               ,JwtClaimTypes.Subject
                               ,JwtClaimTypes.Email
                               ,JwtClaimTypes.Profile
                               ,AGSCommon.CommonConstant.AGSIdentityConstant.AGSFunctionScopeConstant
                            }

                        }
                    }
                }
                ,
                new ApiResource{
                    Name = AGSCommon.CommonConstant.AGSIdentityConstant.AGSDocumentScopeConstant,

                    Scopes =
                    {
                        new Scope()
                        {
                            Name = AGSCommon.CommonConstant.AGSIdentityConstant.AGSDocumentScopeConstant,
                            DisplayName = "Full access to ags.document",
                            UserClaims = {
                                JwtClaimTypes.Name
                               ,JwtClaimTypes.Email
                               ,JwtClaimTypes.Profile
                               ,AGSCommon.CommonConstant.AGSIdentityConstant.AGSFunctionScopeConstant
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
                    ClientId = AGSCommon.CommonConstant.AGSIdentityConstant.AGSClientIdConstant,

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.Implicit,
                    RequireConsent = false,
                    RedirectUris = {
                        "https://localhost:5008/signin-oidc"
                    },
                    PostLogoutRedirectUris =
                    {
                        "https://localhost:5008/signout-callback-oidc"
                    },

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("115500".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = {
                        AGSCommon.CommonConstant.AGSIdentityConstant.AGSDocumentScopeConstant
                        ,AGSCommon.CommonConstant.AGSIdentityConstant.AGSIdentityScopeConstant
                        ,IdentityServerConstants.StandardScopes.OpenId
                        ,IdentityServerConstants.StandardScopes.Profile
                        ,IdentityServerConstants.StandardScopes.Email
                        ,AGSCommon.CommonConstant.AGSIdentityConstant.AGSFunctionScopeConstant
                    },
                    
                    AllowAccessTokensViaBrowser = true,
                    AlwaysIncludeUserClaimsInIdToken = true
                }
            };
        }
    }
}
