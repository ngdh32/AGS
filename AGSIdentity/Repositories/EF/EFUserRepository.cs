using System;
using System.Collections.Generic;
using AGSIdentity.Models;
using AGSIdentity.Models.DataModels;
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

        public User Get(string id)
        {
            var selected = _userManager.FindByIdAsync(id).Result;
            var result = new User(selected);
            return result;
        }

        public List<User> GetAll()
        {
            var users = new List<User>();
            foreach (var user in _applicationDbContext.Users)
            {
                users.Add(new User(user));
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

        public void Create(User user)
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

        public void Update(User user)
        {
            var selected = _userManager.FindByIdAsync(user.Id).Result;
            selected.UserName = user.Username;
            selected.Email = user.Email;
            _ = _userManager.UpdateAsync(selected).Result;
        }
    }
}
