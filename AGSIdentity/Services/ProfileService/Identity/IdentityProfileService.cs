using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AGSIdentity.Models;
using AGSIdentity.Models.EntityModels.EF;
using AGSIdentity.Repositories;
using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace AGSIdentity.Services.ProfileService.Identity
{
    public class IdentityProfileService : IProfileService
    {
        private EFApplicationDbContext _applicationDbContext { get; set; }
        private UserManager<EFApplicationUser> _userManager { get; set; }
        private RoleManager<EFApplicationRole> _roleManager { get; set; }
        private IFunctionClaimRepository _functionClaimRepository { get; set; }

        public IdentityProfileService(
            UserManager<EFApplicationUser> userManager,
            RoleManager<EFApplicationRole> roleManager,
            EFApplicationDbContext applicationDbContext,
            IFunctionClaimRepository functionClaimRepository)
        {
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _functionClaimRepository = functionClaimRepository;
        }

        /// <summary>
        /// add the role and role-related claims to token
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            Console.WriteLine("GetProfileDataAsync fired!");
            var userId = context.Subject.GetSubjectId();
            var user = _userManager.FindByIdAsync(userId).Result;
            var roleNames = _userManager.GetRolesAsync(user).Result;
            foreach(var roleName in roleNames)
            {
                var role = _roleManager.FindByNameAsync(roleName).Result;
                var roleClaims = _roleManager.GetClaimsAsync(role).Result;
                foreach(var roleClaim in roleClaims)
                {
                    // if it is function claim, substitute the ID with the actual claim name
                    // this enable authorization policy 
                    if (roleClaim.Type == AGSCommon.CommonConstant.AGSIdentityConstant.FunctionClaimTypeConstant)
                    {
                        var functionClaim = _functionClaimRepository.Get(roleClaim.Value);
                        if (functionClaim != null)
                        {
                            context.IssuedClaims.Add(new Claim(roleClaim.Type, functionClaim.Name));
                        }
                    }else
                    {
                        context.IssuedClaims.Add(new Claim(roleClaim.Type, roleClaim.Value));
                    }
                    
                }

                // add group
                context.IssuedClaims.Add(new Claim(JwtClaimTypes.Role, roleName));
            }


            context.IssuedClaims.Add(new Claim(JwtClaimTypes.Name, user.UserName));
            context.IssuedClaims.Add(new Claim(JwtClaimTypes.Email, user.Email));

            return Task.CompletedTask;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}
