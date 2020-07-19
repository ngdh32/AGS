using System;
using System.Linq;
using System.Security.Claims;
using AGSIdentity.Models.EntityModels.EF;
using IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace AGSIdentity.Data.EF
{
    public class EFDataSeed : IDataSeed
    {
        private UserManager<EFApplicationUser> _userManager { get; set; }
        private RoleManager<EFApplicationRole> _roleManager { get; set; }
        private EFApplicationDbContext _applicationDbContext { get; set; }
        private ConfigurationDbContext _configurationDbContext { get; set; }
        private IConfiguration _configuration { get; set; }


        public EFDataSeed(UserManager<EFApplicationUser> userManager, RoleManager<EFApplicationRole> roleManager, EFApplicationDbContext applicationDbContext, ConfigurationDbContext configurationDbContext, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _applicationDbContext = applicationDbContext;
            _configurationDbContext = configurationDbContext;
            _configuration = configuration;
        }

        public void InitializeApplicationData()
        {
            EFFunctionClaim identityAdminMenuFC = new EFFunctionClaim()
            {
                Id = AGSCommon.CommonFunctions.GenerateId(),
                Name = AGSCommon.CommonConstant.AGSIdentityConstant.AGSIdentityAdminMenuClaimConstant
            };

            _applicationDbContext.FunctionClaims.Add(identityAdminMenuFC);

            EFFunctionClaim functionClaimMenuFC = new EFFunctionClaim()
            {
                Id = AGSCommon.CommonFunctions.GenerateId(),
                Name = AGSCommon.CommonConstant.AGSIdentityConstant.AGSFunctionClaimMenuClaimConstant
            };


            _applicationDbContext.FunctionClaims.Add(functionClaimMenuFC);

            EFFunctionClaim userMenuFC = new EFFunctionClaim()
            {
                Id = AGSCommon.CommonFunctions.GenerateId(),
                Name = AGSCommon.CommonConstant.AGSIdentityConstant.AGSUserMenuClaimConstant
            };

            _applicationDbContext.FunctionClaims.Add(userMenuFC);


            EFFunctionClaim groupMenuFC = new EFFunctionClaim()
            {
                Id = AGSCommon.CommonFunctions.GenerateId(),
                Name = AGSCommon.CommonConstant.AGSIdentityConstant.AGSGroupMenuClaimConstant
            };

            _applicationDbContext.FunctionClaims.Add(groupMenuFC);

            EFFunctionClaim configMenuFC = new EFFunctionClaim()
            {
                Id = AGSCommon.CommonFunctions.GenerateId(),
                Name = AGSCommon.CommonConstant.AGSIdentityConstant.AGSConfigMenuClaimConstant
            };

            _applicationDbContext.FunctionClaims.Add(configMenuFC);

            _applicationDbContext.FunctionClaims.Add(new EFFunctionClaim() { Id = AGSCommon.CommonFunctions.GenerateId(), Name = AGSCommon.CommonConstant.AGSIdentityConstant.AGSUserEditClaimConstant });
            _applicationDbContext.FunctionClaims.Add(new EFFunctionClaim() { Id = AGSCommon.CommonFunctions.GenerateId(), Name = AGSCommon.CommonConstant.AGSIdentityConstant.AGSGroupEditClaimConstant });
            _applicationDbContext.FunctionClaims.Add(new EFFunctionClaim() { Id = AGSCommon.CommonFunctions.GenerateId(), Name = AGSCommon.CommonConstant.AGSIdentityConstant.AGSFunctionClaimEditClaimConstant });
            _applicationDbContext.FunctionClaims.Add(new EFFunctionClaim() { Id = AGSCommon.CommonFunctions.GenerateId(), Name = AGSCommon.CommonConstant.AGSIdentityConstant.AGSConfigEditClaimConstant });

            

            var userName = AGSCommon.CommonConstant.AGSIdentityConstant.AGSAdminName;
            var email = _configuration["defau" +
                "lt_user_email"];
            var userPassword = _configuration["default_user_password"];
            var adminName = AGSCommon.CommonConstant.AGSIdentityConstant.AGSAdminName;

            var user = new EFApplicationUser
            {
                Id = AGSCommon.CommonFunctions.GenerateId(),
                UserName = userName,
                NormalizedEmail = email,
                NormalizedUserName = userName,
                Email = email,
                First_Name = "Tim",
                Last_Name = "Ng",
                Title = "Developer",
                SecurityStamp = AGSCommon.CommonFunctions.GenerateId(), // need to add this !!!
                PasswordHash = userPassword
            };
            //_ = _userManager.CreateAsync(user, userPassword).Result;
            //user = _userManager.FindByNameAsync(userName).Result;

            _applicationDbContext.Users.Add(user);
            _applicationDbContext.SaveChanges();

            _ = _userManager.AddClaimsAsync(user, new Claim[]{
                                new Claim(JwtClaimTypes.Name, userName),
                                new Claim(JwtClaimTypes.Email, email)
                            }).Result;

            var adminRole = new EFApplicationRole()
            {
                Name = adminName
            };

            _ = _roleManager.CreateAsync(adminRole).Result;
            adminRole = _roleManager.FindByNameAsync(adminRole.Name).Result;
            if (_applicationDbContext.FunctionClaims.Any())
            {
                foreach (var functionClaim in _applicationDbContext.FunctionClaims.ToList())
                {
                    _ = _roleManager.AddClaimAsync(adminRole, new Claim(AGSCommon.CommonConstant.AGSIdentityConstant.FunctionClaimTypeConstant, functionClaim.Id)).Result;
                }

            }
            _ = _userManager.AddToRoleAsync(user, adminRole.Name).Result;

            var user_admin_role = new EFApplicationRole()
            {
                Name = "user_admin_role"
            };

            _ = _roleManager.CreateAsync(user_admin_role).Result;
            user_admin_role = _roleManager.FindByNameAsync(user_admin_role.Name).Result;
            if (_applicationDbContext.FunctionClaims.Any())
            {
                _ = _roleManager.AddClaimAsync(user_admin_role, new Claim(AGSCommon.CommonConstant.AGSIdentityConstant.FunctionClaimTypeConstant, userMenuFC.Id)).Result;
                _ = _roleManager.AddClaimAsync(user_admin_role, new Claim(AGSCommon.CommonConstant.AGSIdentityConstant.FunctionClaimTypeConstant, identityAdminMenuFC.Id)).Result;
            }


            //_applicationDbContext.ConfigValues.Add(new EFConfigValue() { Key = AGSCommon.CommonConstant.AGSIdentityConstant.AGSUserDefaultPasswordConfigKey, IsSecure = true, Value = userPassword });

            _applicationDbContext.SaveChanges();
        }

        public void InitializeAuthenticationServer()
        {
            IdentityServerConfig identityServerConfig = new IdentityServerConfig();

            //configurationDbContext.Database.Migrate();
            if (!_configurationDbContext.Clients.Any())
            {
                foreach (var client in identityServerConfig.GetClients())
                {
                    _configurationDbContext.Clients.Add(client.ToEntity());
                }
                
            }

            if (!_configurationDbContext.IdentityResources.Any())
            {
                foreach (var resource in identityServerConfig.GetIdentityResources())
                {
                    _configurationDbContext.IdentityResources.Add(resource.ToEntity());
                }
            }

            if (!_configurationDbContext.ApiResources.Any())
            {
                foreach (var resource in identityServerConfig.GetApis())
                {
                    _configurationDbContext.ApiResources.Add(resource.ToEntity());
                }
            }

            _configurationDbContext.SaveChanges();
        }

        public void RemoveAuthenticationServerData()
        {

            if (_configurationDbContext.Clients.Any())
            {
                foreach (var client in _configurationDbContext.Clients)
                {
                    _configurationDbContext.Clients.Remove(client);
                }
            }

            if (_configurationDbContext.IdentityResources.Any())
            {
                foreach (var IdentityResource in _configurationDbContext.IdentityResources.ToList())
                {
                    _configurationDbContext.IdentityResources.Remove(IdentityResource);
                }
            }

            if (_configurationDbContext.ApiResources.Any())
            {
                foreach (var ApiResource in _configurationDbContext.ApiResources.ToList())
                {
                    _configurationDbContext.ApiResources.Remove(ApiResource);
                }

            }

            _configurationDbContext.SaveChanges();
        }

        public void RemoveApplicationData()
        {
            if (_userManager.Users.Any())
            {
                foreach (var applicationUser in _userManager.Users.ToList())
                {
                    var claims = _userManager.GetClaimsAsync(applicationUser).Result;
                    if (claims != null)
                    {
                        _ = _userManager.RemoveClaimsAsync(applicationUser, claims).Result;
                    }
                    _ = _userManager.DeleteAsync(applicationUser).Result;
                }
            }

            if (_roleManager.Roles.Any())
            {
                foreach (var applicationRole in _roleManager.Roles.ToList())
                {
                    var claims = _roleManager.GetClaimsAsync(applicationRole).Result;
                    if (claims != null)
                    {
                        foreach(var claim in claims)
                        {
                            _ = _roleManager.RemoveClaimAsync(applicationRole, claim).Result;
                        }
                    }
                    _ = _roleManager.DeleteAsync(applicationRole).Result;
                }
            }

            if (_applicationDbContext.FunctionClaims.Any())
            {
                foreach (var functionClaim in _applicationDbContext.FunctionClaims.ToList())
                {
                    _applicationDbContext.FunctionClaims.Remove(functionClaim);
                }
            }

            if (_applicationDbContext.ConfigValues.Any())
            {
                foreach(var configEntity in _applicationDbContext.ConfigValues.ToList())
                {
                    _applicationDbContext.ConfigValues.Remove(configEntity);
                }
            }

            _applicationDbContext.SaveChanges();
        }
    }
}
