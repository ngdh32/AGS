using System;
using System.Collections.Generic;
using AGSIdentity.Models;
using AGSCommon.Models.DataModels.AGSIdentity;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace AGSIdentity.Repositories.EF
{
    public class EFUserRepository : IUserRepository
    {
        private ApplicationDbContext _applicationDbContext { get; set; }
        private UserManager<ApplicationUser> _userManager { get; set; }

        public EFUserRepository(ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager)
        {
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
        }

        public AGSUser Get(string id)
        {
            var selected = _userManager.FindByIdAsync(id).Result;
            var result = selected.GetAGSUser();
            return result;
        }

        public List<AGSUser> GetAll()
        {
            var users = new List<AGSUser>();
            foreach (var user in _applicationDbContext.Users)
            {
                users.Add(user.GetAGSUser());
            }
            return users;
        }

        public void Delete(string id)
        {
            var selected = _applicationDbContext.Users.Where(x => x.Id == id).FirstOrDefault();
            if (selected != null)
            {
                _ = _userManager.DeleteAsync(selected).Result;
            }
        }

        public void Create(AGSUser user)
        {
            ApplicationUser _user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Email = user.Email,
                UserName = user.Username,
                NormalizedEmail = user.Username
            };


            _ = _userManager.CreateAsync(_user).Result;
        }

        public void Update(AGSUser user)
        {
            var selected = _userManager.FindByIdAsync(user.Id).Result;
            selected.UserName = user.Username;
            selected.Email = user.Email;
            _ = _userManager.UpdateAsync(selected).Result;
        }
    }
}
