using System;
using System.Collections.Generic;
using AGSIdentity.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using AGSIdentity.Models.EntityModels;
using AGSIdentity.Models.EntityModels.AGSIdentity.EF;
using AGSIdentity.Models.EntityModels.AGSIdentity;

namespace AGSIdentity.Repositories.EF
{
    public class EFGroupsRepository : IGroupsRepository
    {
        private readonly EFApplicationDbContext _applicationDbContext;
        private readonly RoleManager<EFApplicationRole> _roleManager;

        public EFGroupsRepository(EFApplicationDbContext applicationDbContext, RoleManager<EFApplicationRole> roleManager)
        {
            _applicationDbContext = applicationDbContext;
            _roleManager = roleManager;
        }

        public string Create(AGSGroupEntity group)
        {
            // create a new role in ASP.NET identity core
            var role = new EFApplicationRole();
            role.Id = CommonConstant.GenerateId(); // assign id here
            MapAGSGroupEntityToEFApplicationRole(group, role);
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
            if (selected == null)
            {
                return null;
            }

            var result = GetAGSGroupEntityFromEFApplicationRole(selected);
            return result;

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
            if (selected == null)
            {
                return 0;
            }

            // update the group info
            MapAGSGroupEntityToEFApplicationRole(group, selected);
            _ = _roleManager.UpdateAsync(selected).Result;

            // remove all the existing associated function claims
            var existingClaimIds = this.GetFunctionClaimIdsByGroupId(group.Id);
            if (existingClaimIds != null)
            {
                foreach (var existingClaimId in existingClaimIds)
                {
                    this.RemoveFunctionClaimFromGroup(group.Id, existingClaimId);
                }
            }

            // add new associated function claims to the group
            if (group.FunctionClaimIds != null)
            {
                foreach (var functionClaimId in group.FunctionClaimIds)
                {
                    this.AddFunctionClaimToGroup(group.Id, functionClaimId);
                }
            }

            return 1;
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
                        if (claim.Type == CommonConstant.FunctionClaimTypeConstant)
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
                _ = _roleManager.AddClaimAsync(selected, new Claim(CommonConstant.FunctionClaimTypeConstant, functionClaimId)).Result;
            }
        }

        public void RemoveFunctionClaimFromGroup(string groupId, string functionClaimId)
        {
            var selected = _roleManager.FindByIdAsync(groupId).Result;
            if (selected != null)
            {
                _ = _roleManager.RemoveClaimAsync(selected, new Claim(CommonConstant.FunctionClaimTypeConstant, functionClaimId)).Result;
            }
        }

        public AGSGroupEntity GetAGSGroupEntityFromEFApplicationRole(EFApplicationRole role)
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

        public void MapAGSGroupEntityToEFApplicationRole(AGSGroupEntity groupEntity, EFApplicationRole efApplicationRole)
        {
            efApplicationRole.Id = groupEntity.Id;
            efApplicationRole.Name = groupEntity.Name;
            efApplicationRole.NormalizedName = groupEntity.Name;
            efApplicationRole.ConcurrencyStamp = CommonConstant.GenerateId();
        }

    }
}
