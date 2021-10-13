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
        public IUsersRepository _userRepository { get; private set; }
        public IGroupsRepository _groupRepository { get; private set; }
        public IFunctionClaimsRepository _functionClaimRepository { get; private set; }
        public IDepartmentsRepository _departmentsRepository { get; private set; }

        public EFRepository(EFApplicationDbContext applicationDbContext
            , UserManager<EFApplicationUser> userManager
            , RoleManager<EFApplicationRole> roleManager
            , IConfiguration configuration,
            SignInManager<EFApplicationUser> signInManager)
        {
            // when all repositories are using the same db context, all the changes and updates can be done
            // in ATOMIC way
            _applicationDbContext = applicationDbContext;

            _userRepository = new EFUsersRepository(_applicationDbContext, userManager, roleManager, signInManager);
            _groupRepository = new EFGroupsRepository(_applicationDbContext, roleManager);
            _functionClaimRepository = new EFFunctionClaimsRepository(_applicationDbContext);
            _departmentsRepository = new EFDepartmentsRepository(_applicationDbContext, userManager);
        }

        public IUsersRepository UsersRepository
        {
            get
            {
                return _userRepository;
            }
        }

        public IGroupsRepository GroupsRepository
        {
            get
            {
                return _groupRepository;
            }
        }

        public IFunctionClaimsRepository FunctionClaimsRepository
        {
            get
            {
                return _functionClaimRepository;
            }
        }

        public IDepartmentsRepository DepartmentsRepository
        {
            get
            {
                return _departmentsRepository;
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
