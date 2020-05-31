using System;
using AGSIdentity.Models;
using Microsoft.AspNetCore.Identity;

namespace AGSIdentity.Repositories.EF
{
    public class EFRepository : IRepository
    {
        private ApplicationDbContext _applicationDbContext { get; set; }
        // make the repository accessible in public but can only be set by EFRepository class
        public IUserRepository userRepository { get; private set; }
        public IGroupRepository groupRepository { get; private set; }
        public IMenuRepository menuRepository { get; private set; }
        public IFunctionClaimRepository functionClaimRepository { get; private set; }

        public EFRepository(ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _applicationDbContext = applicationDbContext;

            userRepository = new EFUserRepository(_applicationDbContext, userManager);
            groupRepository = new EFGroupRepository(_applicationDbContext, roleManager);
            menuRepository = new EFMenuRepository(_applicationDbContext);
            functionClaimRepository = new EFFunctionClaimRepository(_applicationDbContext);
        }

        public int Save()
        {
            return _applicationDbContext.SaveChanges();
        }

        public void Dispose()
        {
            _applicationDbContext.Dispose();
        }
    }
}
