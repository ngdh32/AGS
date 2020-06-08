using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AGSIdentity.Models;
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
        private ApplicationDbContext _applicationDbContext { get; set; }
        private UserManager<ApplicationUser> _userManager { get; set; }
        private RoleManager<IdentityRole> _roleManager { get; set; }

        public IdentityProfileService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<IdentityOptions> optionsAccessor,
            ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        /// <summary>
        /// add the role and role-related claims to token
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            Console.WriteLine("GetProfileDataAsync fired!");
            var userId = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(userId);
            var roleNames = await _userManager.GetRolesAsync(user);
            foreach(var roleName in roleNames)
            {
                Console.WriteLine("roleName:" + roleName);
                var role = await _roleManager.FindByNameAsync(roleName);
                var roleClaims = await _roleManager.GetClaimsAsync(role);
                foreach(var roleClaim in roleClaims)
                {
                    Console.WriteLine("roleClaim:" + roleClaim);
                    context.IssuedClaims.Add(new Claim(roleClaim.Type, roleClaim.Value));
                }
                context.IssuedClaims.Add(new Claim(JwtClaimTypes.Role, roleName));
            }

        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}
