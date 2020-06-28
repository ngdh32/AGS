using System;
using System.Collections.Generic;
using AGSIdentity.Models;
using AGSCommon.Models.EntityModels.AGSIdentity;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using AGSIdentity.Models.EntityModels.EF;
using Microsoft.Extensions.Configuration;
using AGSIdentity.Models.EntityModels;
using System.Security.Claims;
using IdentityModel;
using AGSCommon.Models.ViewModels.AGSIdentity;

namespace AGSIdentity.Repositories.EF
{
    public class EFUserRepository : IUserRepository
    {
        private EFApplicationDbContext _applicationDbContext { get; set; }
        private UserManager<EFApplicationUser> _userManager { get; set; }
        private RoleManager<EFApplicationRole> _roleManager { get; set; }
        private IConfiguration _configuration { get; set; }

        public EFUserRepository(EFApplicationDbContext applicationDbContext, UserManager<EFApplicationUser> userManager, RoleManager<EFApplicationRole> roleManager, IConfiguration configuration)
        {
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public AGSUserEntity Get(string id)
        {
            var selected = _userManager.FindByIdAsync(id).Result;
            if (selected != null)
            {
                var result = GetAGSUserEntity(selected);
                return result;
            }
            else
            {
                return null;
            }
        }

        public List<string> GetAll()
        {
            var result = new List<string>();
            foreach (var user in _applicationDbContext.Users)
            {
                result.Add(user.Id);
            }
            return result;
        }

        public void Delete(string id)
        {
            var selected = _applicationDbContext.Users.Where(x => x.Id == id).FirstOrDefault();
            if (selected != null)
            {
                _ = _userManager.DeleteAsync(selected).Result;
            }
        }

        public string Create(AGSUserWithPasswordModel user)
        {
            // creat the Identity User with the provided password
            EFApplicationUser appUser = GetApplicationUser(user);

            _ = _userManager.CreateAsync(appUser, user.Password).Result;

            _ = _userManager.AddClaimsAsync(appUser, new Claim[]{
                                new Claim(JwtClaimTypes.Name, user.Username),
                                new Claim(JwtClaimTypes.Email, user.Email)
                            }).Result;

            // update the associated groups
            if (user.GroupIds != null)
            {
                foreach(var groupId in user.GroupIds)
                {
                    this.AddUserToGroup(user.Id, groupId);
                }
            }

            return appUser.Id;
        }

        public int Update(AGSUserWithPasswordModel user)
        {
            var selected = _userManager.FindByIdAsync(user.Id).Result;
            if (selected != null)
            {
                selected = GetApplicationUser(user);

                // change the password
                var resetPasswordToken = _userManager.GeneratePasswordResetTokenAsync(selected).Result;
                _ = _userManager.ResetPasswordAsync(selected, resetPasswordToken, user.Password).Result;

                _ = _userManager.UpdateAsync(selected).Result;

                // remove all existing claims associated with user
                var existingClaims = _userManager.GetClaimsAsync(selected).Result;
                if (existingClaims != null)
                {
                    _ = _userManager.RemoveClaimsAsync(selected, existingClaims).Result;
                }

                _ = _userManager.AddClaimsAsync(selected, new Claim[]{
                                new Claim(JwtClaimTypes.Name, user.Username),
                                new Claim(JwtClaimTypes.Email, user.Email)
                            }).Result;


                // remove all the existing associated groups
                var existingGroupIds = this.GetGroupIdsByUser(selected.Id);
                if (existingGroupIds != null)
                {
                    foreach(var existingGroupId in existingGroupIds)
                    {
                        this.RemoveUserFromGroup(selected.Id, existingGroupId);
                    }
                }


                // add the new associated groups
                if (user.GroupIds != null)
                {
                    foreach(var groupId in user.GroupIds)
                    {
                        this.AddUserToGroup(selected.Id, groupId);
                    }
                }

                return 1;
            }
            else
            {
                return 0;
            }
        }

        public List<string> GetGroupIdsByUser(string userId)
        {
            var result = new List<string>();
            var selected = _userManager.FindByIdAsync(userId).Result;
            if (selected != null)
            {
                var roleNames = _userManager.GetRolesAsync(selected).Result;
                foreach(var roleName in roleNames)
                {
                    var role = _roleManager.FindByNameAsync(roleName).Result;
                    if (role != null)
                    {
                        result.Add(role.Id);
                    }
                }
            }
            return result;
        }

        public void AddUserToGroup(string userId, string groupId)
        {
            var selectedRole = _roleManager.FindByIdAsync(groupId).Result;
            var selectedUser = _userManager.FindByIdAsync(userId).Result;
            if (selectedRole != null && selectedUser != null)
            {
                _ = _userManager.AddToRoleAsync(selectedUser, selectedRole.Name).Result;
            }
        }

        public void RemoveUserFromGroup(string userId, string groupId)
        {
            var selectedRole = _roleManager.FindByIdAsync(groupId).Result;
            var selectedUser = _userManager.FindByIdAsync(userId).Result;
            if (selectedRole != null && selectedUser != null)
            {
                _ = _userManager.RemoveFromRoleAsync(selectedUser, selectedRole.Name).Result;
            }
        }

        public EFApplicationUser GetApplicationUser(AGSUserEntity userEntity)
        {
            var result = new EFApplicationUser()
            {
                Id = userEntity.Id,
                UserName = userEntity.Username,
                NormalizedUserName = userEntity.Username,
                Email = userEntity.Email,
                NormalizedEmail = userEntity.Email,
                SecurityStamp = userEntity.Id
            };

            return result;
        }

        public AGSUserEntity GetAGSUserEntity(EFApplicationUser user)
        {
            var result = new AGSUserEntity()
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.UserName,
                GroupIds = new List<string>()
            };

            // get the associated groups
            var groupIds = this.GetGroupIdsByUser(user.Id);
            if (groupIds != null)
            {
                result.GroupIds = groupIds;
            }

            return result;
        }
    }
}
