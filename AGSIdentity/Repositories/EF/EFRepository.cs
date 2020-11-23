using System;
using AGSIdentity.Models;
using AGSIdentity.Models.EntityModels.AGSIdentity.EF;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace AGSIdentity.Repositories.EF
{
    public class EFRepository : IRepository
    {
        private EFApplicationDbContext _applicationDbContext { get; set; }
        // make the repository accessible in public but can only be set by EFRepository class
        public IUserRepository _userRepository { get; private set; }
        public IGroupRepository _groupRepository { get; private set; }
        public IFunctionClaimRepository _functionClaimRepository { get; private set; }

        public EFRepository(EFApplicationDbContext applicationDbContext, UserManager<EFApplicationUser> userManager, RoleManager<EFApplicationRole> roleManager, IConfiguration configuration, SignInManager<EFApplicationUser> signInManager)
        {
            _applicationDbContext = applicationDbContext;

            _userRepository = new EFUserRepository(_applicationDbContext, userManager, roleManager, signInManager);
            _groupRepository = new EFGroupRepository(_applicationDbContext, roleManager);
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

        public static DatabaseTypeEnum GetDatabaseTypeEnum(string database_type)
        {
            if (string.IsNullOrEmpty(database_type))
            {
                throw new ArgumentNullException();
            }

            switch (database_type.ToUpper())
            {
                case "MSSQL":
                    return DatabaseTypeEnum.mssql;
                case "MYSQL":
                    return DatabaseTypeEnum.mysql;
                default:
                    throw new ArgumentException();
            }
        }

        public enum DatabaseTypeEnum
        {
            mssql,
            mysql
        }
    }
}
