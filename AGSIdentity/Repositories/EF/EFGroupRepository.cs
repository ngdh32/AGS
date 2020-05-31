using System;
using System.Collections.Generic;
using AGSIdentity.Models;
using AGSIdentity.Models.DataModels;
using Microsoft.AspNetCore.Identity;

namespace AGSIdentity.Repositories.EF
{
    public class EFGroupRepository : IGroupRepository
    {
        private ApplicationDbContext _applicationDbContext { get; set; }
        private RoleManager<IdentityRole> _roleManager { get; set; }

        public EFGroupRepository(ApplicationDbContext applicationDbContext, RoleManager<IdentityRole> roleManager)
        {
            _applicationDbContext = applicationDbContext;
            _roleManager = roleManager;
        }

        public void Create(Group group)
        {
            var identityRole = new IdentityRole()
            {
                Id = Guid.NewGuid().ToString(),
                Name = group.Name,
                NormalizedName = group.Name
            };
            _ = _roleManager.CreateAsync(identityRole).Result;
        }

        public void Delete(string id)
        {
            var selected = _roleManager.FindByIdAsync(id).Result;
            if (selected != null)
            {
                _ = _roleManager.DeleteAsync(selected).Result;
            }
        }

        public Group Get(string id)
        {
            var selected = _roleManager.FindByIdAsync(id).Result;
            var result = new Group(selected);
            return result;
        }

        public List<Group> GetAll()
        {
            var groups = new List<Group>();
            foreach(var role in _applicationDbContext.Roles)
            {
                groups.Add(new Group(role));
            }
            return groups;

        }

        public void Update(Group group)
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
