using System;
using AGSIdentity.Models;
using AGSIdentity.Models.EntityModels.EF;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace AGSIdentity.Repositories.EF
{
    public class EFRepository : IRepository
    {
        private ApplicationDbContext _applicationDbContext { get; set; }
        // make the repository accessible in public but can only be set by EFRepository class
        public IUserRepository _userRepository { get; private set; }
        public IGroupRepository _groupRepository { get; private set; }
        public IMenuRepository _menuRepository { get; private set; }
        public IFunctionClaimRepository _functionClaimRepository { get; private set; }

        public EFRepository(ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IConfiguration configuration)
        {
            _applicationDbContext = applicationDbContext;

            _userRepository = new EFUserRepository(_applicationDbContext, userManager, roleManager, configuration);
            _groupRepository = new EFGroupRepository(_applicationDbContext, roleManager, userManager);
            _menuRepository = new EFMenuRepository(_applicationDbContext);
            _functionClaimRepository = new EFFunctionClaimRepository(_applicationDbContext);
        }

        public IUserRepository UserRepository
        {
            get
            {
                return _userRepository;
            }
        }

        public IGroupRepository GroupRepository
        {
            get
            {
                return _groupRepository;
            }
        }

        public IMenuRepository MenuRepository
        {
            get
            {
                return _menuRepository;
            }
        }

        public IFunctionClaimRepository FunctionClaimRepository
        {
            get
            {
                return _functionClaimRepository;
            }
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
