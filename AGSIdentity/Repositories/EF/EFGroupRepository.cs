using System;
using System.Collections.Generic;
using AGSIdentity.Models;
using AGSCommon.Models.EntityModels.AGSIdentity;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using AGSIdentity.Models.EntityModels.EF;
using AGSIdentity.Models.EntityModels;

namespace AGSIdentity.Repositories.EF
{
    public class EFGroupRepository : IGroupRepository
    {
        private EFApplicationDbContext _applicationDbContext { get; set; }
        private RoleManager<EFApplicationRole> _roleManager { get; set; }
        private UserManager<EFApplicationUser> _userManager { get; set; }

        public EFGroupRepository(EFApplicationDbContext applicationDbContext, RoleManager<EFApplicationRole> roleManager, UserManager<EFApplicationUser> userManager)
        {
            _applicationDbContext = applicationDbContext;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public string Create(AGSGroupEntity group)
        {
            // create a new role in ASP.NET identity core
            var role = new EFApplicationRole();
            UpdateApplicationRole(group, role);
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
                var result = GetAGSGroupEntity(selected);
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

        public int Update(AGSGroupEntity group)
        {
            var selected = _roleManager.FindByIdAsync(group.Id).Result;
            if (selected != null)
            {
                // update the group info
                UpdateApplicationRole(group, selected);
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

                return 1;
            }else
            {
                return 0;
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

        public AGSGroupEntity GetAGSGroupEntity(EFApplicationRole role)
        {
            var result = new AGSGroupEntity()
            {
                Id = role.Id,
                Name = role.Name,
                FunctionClaimIds = new List<string>()
            };

            var functionClaimIds = this.GetFunctionClaimIdsByGroupId(role.Id);
            if (functionClaimIds != null)
            {
                result.FunctionClaimIds = functionClaimIds;
            }

            return result;
        }

        public void UpdateApplicationRole(AGSGroupEntity groupEntity, EFApplicationRole efApplicationRole)
        {
            efApplicationRole.Id = groupEntity.Id;
            efApplicationRole.Name = groupEntity.Name;
            efApplicationRole.NormalizedName = groupEntity.Name;
            efApplicationRole.ConcurrencyStamp = AGSCommon.CommonFunctions.GenerateId();
        }

    }
}
