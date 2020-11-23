using System;
using System.Linq;
using System.Security.Claims;
using AGSIdentity.Models.EntityModels.AGSIdentity.EF;
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


        private const string AGSAdminName = "admin";
        
        


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
            // add all function claims into Database
            var ags_identity_constant_type = typeof(CommonConstant);
            var constant_fields = ags_identity_constant_type.GetFields();
            foreach (var constant_field in constant_fields)
            {
                if (constant_field.Name.EndsWith("ClaimConstant"))
                {
                    var claimValue = (string)(constant_field.GetValue(null));
                    _applicationDbContext.FunctionClaims.Add(new EFFunctionClaim() { Id = CommonConstant.GenerateId(), Name = claimValue });
                }
            }

            // create admin user 
            var userName = AGSAdminName;
            var email = _configuration["default_user_email"];
            var userPassword = _configuration["default_user_password"];

            var user = new EFApplicationUser
            {
                Id = CommonConstant.GenerateId(),
                UserName = userName,
                NormalizedEmail = email,
                NormalizedUserName = userName,
                Email = email,
                First_Name = "Tim",
                Last_Name = "Ng",
                Title = "Developer",
                SecurityStamp = CommonConstant.GenerateId(), // need to add this !!!
            };
            _ = _userManager.CreateAsync(user, userPassword).Result;
            _applicationDbContext.SaveChanges();
        }

        public void InitializeAuthenticationServer()
        {
            IdentityServerConfig identityServerConfig = new IdentityServerConfig(_configuration);

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

            _applicationDbContext.SaveChanges();
        }
    }
}
