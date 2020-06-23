using System;
using System.Collections.Generic;
using AGSIdentity.Models;
using AGSCommon.Models.DataModels.AGSIdentity;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using AGSIdentity.Models.EntityModels.EF;
using AGSIdentity.Models.EntityModels;

namespace AGSIdentity.Repositories.EF
{
    public class EFGroupRepository : IGroupRepository
    {
        private ApplicationDbContext _applicationDbContext { get; set; }
        private RoleManager<ApplicationRole> _roleManager { get; set; }
        private UserManager<ApplicationUser> _userManager { get; set; }

        public EFGroupRepository(ApplicationDbContext applicationDbContext, RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _applicationDbContext = applicationDbContext;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public string Create(AGSGroupEntity group)
        {
            // create a new role in ASP.NET identity core
            var role = new ApplicationRole()
            {
                Id = group.Id,
                Name = group.Name,
                NormalizedName = group.Name
            };
            _ = _roleManager.CreateAsync(role).Result;

            // update the associated Function Claims
            if (group.FunctionClaimIds != null)
            {
                foreach (var functionClaimId in group.FunctionClaimIds)
                {
                    this.AddFunctionClaimToGroup(group.Id, functionClaimId);
                }   
            }

            return role.Id;
        }

        public void Delete(string id)
        {
            var selected = _roleManager.FindByIdAsync(id).Result;
            if (selected != null)
            {
                _ = _roleManager.DeleteAsync(selected).Result;
            }
        }

        public AGSGroupEntity Get(string id)
        {
            var selected = _roleManager.FindByIdAsync(id).Result;
            if (selected != null)
            {
                var result = new AGSGroupEntity()
                {
                    Id = selected.Id,
                    Name = selected.Name,
                    FunctionClaimIds = new List<string>()
                };

                var functionClaimIds = this.GetFunctionClaimIdsByGroupId(id);
                if (functionClaimIds != null)
                {
                    result.FunctionClaimIds = functionClaimIds;
                }

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
            foreach(var role in _applicationDbContext.Roles)
            {
                var group = role.Id;
                result.Add(group) ;
            }
            return result;

        }

        public void Update(AGSGroupEntity group)
        {
            var selected = _roleManager.FindByIdAsync(group.Id).Result;
            if (selected != null)
            {
                // update the group info
                selected.Name = group.Name;
                selected.NormalizedName = group.Name;
                _ = _roleManager.UpdateAsync(selected).Result;

                // remove all the existing associated function claims
                var existingClaimIds = this.GetFunctionClaimIdsByGroupId(group.Id);
                if (existingClaimIds != null)
                {
                    foreach(var existingClaimId in existingClaimIds)
                    {
                        this.RemoveFunctionClaimFromGroup(group.Id, existingClaimId);
                    }
                }

                // add new associated function claims to the group
                if (group.FunctionClaimIds != null)
                {
                    foreach(var functionClaimId in group.FunctionClaimIds)
                    {
                        this.AddFunctionClaimToGroup(group.Id, functionClaimId);
                    }
                }
            }
        }

        public List<string> GetFunctionClaimIdsByGroupId(string groupId)
        {
            List<string> result = new List<string>();
            var selected = _roleManager.FindByIdAsync(groupId).Result;
            if (selected != null)
            {
                var claims = _roleManager.GetClaimsAsync(selected).Result;
                if (claims != null)
                {
                    foreach (var claim in claims)
                    {
                        if (claim.Type == AGSCommon.CommonConstant.AGSIdentityConstant.FunctionClaimTypeConstant)
                        {
                            result.Add(claim.Value);
                        }
                    }
                }
                
            }
            return result;
        }

        public void AddFunctionClaimToGroup(string groupId, string functionClaimId)
        {
            var selected = _roleManager.FindByIdAsync(groupId).Result;
            if (selected != null)
            {
                _ = _roleManager.AddClaimAsync(selected, new Claim(AGSCommon.CommonConstant.AGSIdentityConstant.FunctionClaimTypeConstant, functionClaimId)).Result;
            }
        }

        public void RemoveFunctionClaimFromGroup(string groupId, string functionClaimId)
        {
            var selected = _roleManager.FindByIdAsync(groupId).Result;
            if (selected != null)
            {
                _ = _roleManager.RemoveClaimAsync(selected, new Claim(AGSCommon.CommonConstant.AGSIdentityConstant.FunctionClaimTypeConstant, functionClaimId)).Result;
            }
        }
    }
}
