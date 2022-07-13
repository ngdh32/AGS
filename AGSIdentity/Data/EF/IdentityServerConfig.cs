using System;
using System.Collections.Generic;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;

namespace AGSIdentity.Data.EF
{
    public class IdentityServerConfig
    {
        private IConfiguration _configuration { get; set; }

        public IdentityServerConfig(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEnumerable<IdentityResource> GetIdentityResources()
        {
            // add roles and role-related claims to identity resource
            var roleResource = new IdentityResource(
                name: CommonConstant.AGSFunctionClaimResouceConstant,
                displayName: "User's roles and role-related claims",
                claimTypes: new[] { CommonConstant.FunctionClaimTypeConstant });

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
                               ,CommonConstant.AGSFunctionClaimResouceConstant
                            }

                        }
                    }
                }
                ,
                new ApiResource{
                    Name = CommonConstant.AGSDocumentScopeConstant,

                    Scopes =
                    {
                        new Scope()
                        {
                            Name = CommonConstant.AGSDocumentScopeConstant,
                            DisplayName = "Full access to ags.document",
                            UserClaims = {
                                JwtClaimTypes.Id
                               ,JwtClaimTypes.Name
                               ,JwtClaimTypes.Email
                               ,JwtClaimTypes.Profile
                               ,CommonConstant.AGSFunctionClaimResouceConstant
                            }

                        }
                    }
                }
            };
        }

        public IEnumerable<Client> GetClients()
        {
            List<Claim> claims = new List<Claim>();
            // add all asg-identity related function claims into Database
            var ags_identity_constant_type = typeof(CommonConstant);
            var constant_fields = ags_identity_constant_type.GetFields();
            foreach (var constant_field in constant_fields)
            {
                if (constant_field.Name.EndsWith("ClaimConstant"))
                {
                    var claimValue = (string)(constant_field.GetValue(null));
                    claims.Add(new Claim(CommonConstant.AGSFunctionClaimResouceConstant, claimValue));
                }
            }

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
                        CommonConstant.AGSDocumentScopeConstant
                        ,CommonConstant.AGSIdentityScopeConstant
                        ,IdentityServerConstants.StandardScopes.OpenId
                        ,IdentityServerConstants.StandardScopes.Profile
                        ,IdentityServerConstants.StandardScopes.Email
                        ,CommonConstant.AGSFunctionClaimResouceConstant
                    },
                    AllowAccessTokensViaBrowser = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    RequirePkce = true
                },
                new Client
                {
                    ClientId = CommonConstant.AGSClientIdConstant + "_Test",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    RequireConsent = false,
                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret(_configuration["ags_web_secret"].Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = {
                        CommonConstant.AGSDocumentScopeConstant
                        ,CommonConstant.AGSIdentityScopeConstant
                        ,IdentityServerConstants.StandardScopes.OpenId
                        ,IdentityServerConstants.StandardScopes.Profile
                        ,IdentityServerConstants.StandardScopes.Email
                        ,CommonConstant.AGSFunctionClaimResouceConstant
                    },
                    AllowAccessTokensViaBrowser = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                },
                new Client
                {
                    ClientId = CommonConstant.AGSClientIdConstant + "_Document",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    RequireConsent = false,
                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret(_configuration["ags_web_secret"].Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = {
                        CommonConstant.AGSIdentityScopeConstant
                    },
                    AllowAccessTokensViaBrowser = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    Claims = claims 
                }
            };
        }
    }
}