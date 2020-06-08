using System;
using System.Collections.Generic;
using AGSIdentity.Models;
using AGSCommon.Models.DataModels.AGSIdentity;
using Microsoft.AspNetCore.Identity;

namespace AGSIdentity.Repositories.EF
{
    public class EFGroupRepository : IGroupRepository
    {
        private ApplicationDbContext _applicationDbContext { get; set; }
        private RoleManager<ApplicationRole> _roleManager { get; set; }

        public EFGroupRepository(ApplicationDbContext applicationDbContext, RoleManager<ApplicationRole> roleManager)
        {
            _applicationDbContext = applicationDbContext;
            _roleManager = roleManager;
        }

        public void Create(AGSGroup group)
        {
            var ApplicationRole = new ApplicationRole()
            {
                Id = Guid.NewGuid().ToString(),
                Name = group.Name,
                NormalizedName = group.Name
            };
            _ = _roleManager.CreateAsync(ApplicationRole).Result;
        }

        public void Delete(string id)
        {
            var selected = _roleManager.FindByIdAsync(id).Result;
            if (selected != null)
            {
                _ = _roleManager.DeleteAsync(selected).Result;
            }
        }

        public AGSGroup Get(string id)
        {
            var selected = _roleManager.FindByIdAsync(id).Result;
            var result = selected.GetAGSGroup();
            return result;
        }

        public List<AGSGroup> GetAll()
        {
            var groups = new List<AGSGroup>();
            foreach(var role in _applicationDbContext.Roles)
            {
                groups.Add(role.GetAGSGroup()) ;
            }
            return groups;

        }

        public void Update(AGSGroup group)
        {
            var selected = _roleManager.FindByIdAsync(group.Id).Result;
            if (selected != null)
            {
                selected.Name = group.Name;
                selected.NormalizedName = group.Name;
                _ = _roleManager.UpdateAsync(selected).Result;
            }
        }
    }
}
