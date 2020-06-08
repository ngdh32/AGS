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
                    Console.WriteLine("roleClaim:" + roleClaim);
                    context.IssuedClaims.Add(new Claim(roleClaim.Type, roleClaim.Value));
                }
                context.IssuedClaims.Add(new Claim(JwtClaimTypes.Role, roleName));
            }

            Console.WriteLine("context.IssuedClaims");
            foreach(var claim in context.IssuedClaims){
                Console.WriteLine($"{claim.Type} : {claim.Value}");
            }

            Console.WriteLine("context.Subject.Claims");
            foreach(var claim in context.Subject.Claims){
                Console.WriteLine($"{claim.Type} : {claim.Value}");
            }

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
