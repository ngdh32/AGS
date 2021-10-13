using System.Collections.Generic;
using System.Linq;
using AGSIdentity.Models.EntityModels.AGSIdentity.EF;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace AGSIdentity.Data.EF
{
    public class EFDataSeed : IDataSeed
    {
        private readonly UserManager<EFApplicationUser> _userManager;
        private readonly RoleManager<EFApplicationRole> _roleManager;
        private readonly EFApplicationDbContext _applicationDbContext;
        private readonly ConfigurationDbContext _configurationDbContext;
        private readonly IConfiguration _configuration;
        private readonly string _defaultPassword;
        private const string _normalUserGroupName = "Normal User";


        private const string AGSAdminName = "admin";

        public EFDataSeed(UserManager<EFApplicationUser> userManager, RoleManager<EFApplicationRole> roleManager, EFApplicationDbContext applicationDbContext, ConfigurationDbContext configurationDbContext, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _applicationDbContext = applicationDbContext;
            _configurationDbContext = configurationDbContext;
            _configuration = configuration;
            _defaultPassword = _configuration["default_user_password"];
        }

        public void InitializeApplicationData()
        {
            // create admin user 
            var userName = AGSAdminName;
            var email = _configuration["default_user_email"];

            var user = new EFApplicationUser
            {
                Id = CommonConstant.GenerateId(),
                UserName = userName,
                NormalizedEmail = email,
                NormalizedUserName = userName,
                Email = email,
                SecurityStamp = CommonConstant.GenerateId(), // need to add this !!!
            };
            _ = _userManager.CreateAsync(user, _defaultPassword).Result;

            // add all asg-identity related function claims into Database
            var ags_identity_constant_type = typeof(CommonConstant);
            var constant_fields = ags_identity_constant_type.GetFields();
            var agsUserChangePasswordClaimConstantId = "";
            foreach (var constant_field in constant_fields)
            {
                if (constant_field.Name.EndsWith("ClaimConstant"))
                {
                    var claimValue = (string)(constant_field.GetValue(null));
                    var functionClaimId = CommonConstant.GenerateId();
                    if (claimValue == CommonConstant.AGSUserChangePasswordClaimConstant)
                    {
                        agsUserChangePasswordClaimConstantId = functionClaimId;
                    }
                    _applicationDbContext.FunctionClaims.Add(new EFFunctionClaim() { Id = functionClaimId, Name = claimValue });
                }
            }
            _applicationDbContext.SaveChanges();

            var normalUserGroup = new EFApplicationRole
            {
                Id = CommonConstant.GenerateId(),
                Name = _normalUserGroupName,
                NormalizedName = _normalUserGroupName,
                ConcurrencyStamp = CommonConstant.GenerateId()
            };

            _ = _roleManager.CreateAsync(normalUserGroup).Result;
            _ = _roleManager.AddClaimAsync(normalUserGroup, new System.Security.Claims.Claim(CommonConstant.FunctionClaimTypeConstant, agsUserChangePasswordClaimConstantId)).Result;

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
                _applicationDbContext.FunctionClaims.RemoveRange(_applicationDbContext.FunctionClaims);
            }

            if (_applicationDbContext.UserDepartments.Any())
            {
                _applicationDbContext.UserDepartments.RemoveRange(_applicationDbContext.UserDepartments);
            }

            if (_applicationDbContext.Departments.Any())
            {
                _applicationDbContext.Departments.RemoveRange(_applicationDbContext.Departments);
            }

            _applicationDbContext.SaveChanges();
        }

        public void AddSampleDataIntoDatabase()
        {
            var userTim = new EFApplicationUser
            {
                Id = CommonConstant.GenerateId(),
                UserName = "tim",
                NormalizedEmail = "tim@ags.com",
                NormalizedUserName = "tim",
                Email = "tim@ags.com",
                SecurityStamp = CommonConstant.GenerateId(), // need to add this !!!
            };
            _ = _userManager.CreateAsync(userTim, _defaultPassword).Result;

            var userTom = new EFApplicationUser
            {
                Id = CommonConstant.GenerateId(),
                UserName = "tom",
                NormalizedEmail = "tom@ags.com",
                NormalizedUserName = "tom",
                Email = "tom@ags.com",
                SecurityStamp = CommonConstant.GenerateId(), // need to add this !!!
            };
            _ = _userManager.CreateAsync(userTom, _defaultPassword).Result;

            var userChris = new EFApplicationUser
            {
                Id = CommonConstant.GenerateId(),
                UserName = "chris",
                NormalizedEmail = "chris@ags.com",
                NormalizedUserName = "chris",
                Email = "chris@ags.com",
                SecurityStamp = CommonConstant.GenerateId(), // need to add this !!!
            };
            _ = _userManager.CreateAsync(userChris, _defaultPassword).Result;

            var userJack = new EFApplicationUser
            {
                Id = CommonConstant.GenerateId(),
                UserName = "jack",
                NormalizedEmail = "jack@ags.com",
                NormalizedUserName = "jack",
                Email = "jack@ags.com",
                SecurityStamp = CommonConstant.GenerateId(), // need to add this !!!
            };
            _ = _userManager.CreateAsync(userJack, _defaultPassword).Result;

            var userPeter = new EFApplicationUser
            {
                Id = CommonConstant.GenerateId(),
                UserName = "peter",
                NormalizedEmail = "peter@ags.com",
                NormalizedUserName = "peter",
                Email = "peter@ags.com",
                SecurityStamp = CommonConstant.GenerateId(), // need to add this !!!
            };
            _ = _userManager.CreateAsync(userPeter, _defaultPassword).Result;

            var userLouis = new EFApplicationUser
            {
                Id = CommonConstant.GenerateId(),
                UserName = "louis",
                NormalizedEmail = "louis@ags.com",
                NormalizedUserName = "louis",
                Email = "louis@ags.com",
                SecurityStamp = CommonConstant.GenerateId(), // need to add this !!!
            };
            _ = _userManager.CreateAsync(userLouis, _defaultPassword).Result;

            var itDepartment = new EFDepartment()
            {
                Id = CommonConstant.GenerateId(),
                Name = "IT Department",
                ParentDepartmentId = null,
                HeadUserId = userChris.Id,
            };

            var itApplicationDepartment = new EFDepartment()
            {
                Id = CommonConstant.GenerateId(),
                Name = "IT Application Department",
                ParentDepartmentId = itDepartment.Id,
                HeadUserId = userJack.Id,
            };

            var hrDepartment = new EFDepartment()
            {
                Id = CommonConstant.GenerateId(),
                Name = "IT Application Department",
                ParentDepartmentId = itDepartment.Id,
                HeadUserId = userLouis.Id,
            };

            _applicationDbContext.Departments.Add(itDepartment);
            _applicationDbContext.Departments.Add(itApplicationDepartment);
            _applicationDbContext.Departments.Add(hrDepartment);    

            // add users into deparments
            _applicationDbContext.UserDepartments.Add(new EFApplicationUserDepartment()
            {
                Department = itDepartment,
                User = userTim
            });

            _applicationDbContext.UserDepartments.Add(new EFApplicationUserDepartment()
            {
                Department = itApplicationDepartment,
                User = userTim
            });

            _applicationDbContext.UserDepartments.Add(new EFApplicationUserDepartment()
            {
                Department = itDepartment,
                User = userTom
            });

            _applicationDbContext.UserDepartments.Add(new EFApplicationUserDepartment()
            {
                Department = itDepartment,
                User = userJack
            });

            _applicationDbContext.UserDepartments.Add(new EFApplicationUserDepartment()
            {
                Department = itApplicationDepartment,
                User = userJack
            });

            _applicationDbContext.UserDepartments.Add(new EFApplicationUserDepartment()
            {
                Department = itDepartment,
                User = userChris
            });

            _applicationDbContext.UserDepartments.Add(new EFApplicationUserDepartment()
            {
                Department = hrDepartment,
                User = userLouis
            });

            _applicationDbContext.UserDepartments.Add(new EFApplicationUserDepartment()
            {
                Department = hrDepartment,
                User = userPeter
            });


            // add all users into group Normal_user
            _userManager.AddToRoleAsync(userTim, _normalUserGroupName).Wait();
            _userManager.AddToRoleAsync(userTom, _normalUserGroupName).Wait();
            _userManager.AddToRoleAsync(userChris, _normalUserGroupName).Wait();
            _userManager.AddToRoleAsync(userJack, _normalUserGroupName).Wait();
            _userManager.AddToRoleAsync(userPeter, _normalUserGroupName).Wait();
            _userManager.AddToRoleAsync(userLouis, _normalUserGroupName).Wait();
            
            _applicationDbContext.SaveChanges();
        }
    }
}
