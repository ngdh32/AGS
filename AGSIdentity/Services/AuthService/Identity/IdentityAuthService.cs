using System;
using System.Security.Claims;
using AGSIdentity.Models.ViewModels.Pages.Login;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System.Net;
using AGSIdentity.Models.EntityModels.AGSIdentity.EF;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AGSIdentity.Services.AuthService.Identity
{
    public class IdentityAuthService : IAuthService
    {
        private SignInManager<EFApplicationUser> _signInManager { get; set; }
        private UserManager<EFApplicationUser> _userManager { get; set; }
        private IIdentityServerInteractionService _interactionService { get; set; }

        public IdentityAuthService(SignInManager<EFApplicationUser> signInManager, UserManager<EFApplicationUser> userManager, IIdentityServerInteractionService interactionService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _interactionService = interactionService;
        }

        public bool Login(LoginInputModel loginInputModel)
        {
            bool isLoginSuccessful = true;

            if (loginInputModel == null)
            {
                isLoginSuccessful = false;
            }
            else if (string.IsNullOrEmpty(loginInputModel.password))
            {
                isLoginSuccessful = false;
            }

            if (isLoginSuccessful)
            {
                var selected = _userManager.FindByNameAsync(loginInputModel.username).Result;
                if (selected != null)
                {
                    var result = _signInManager.PasswordSignInAsync(selected, loginInputModel.password, true, false).Result;
                    isLoginSuccessful = result.Succeeded;
                }
                else
                {
                    isLoginSuccessful = false;
                }
            }

            return isLoginSuccessful;

        }

        
        public LogoutContext GetLogoutContext(string logoutId)
        {
            if (string.IsNullOrEmpty(logoutId))
            {
                logoutId = WebUtility.UrlDecode(logoutId);
            }

            var context = _interactionService.GetLogoutContextAsync(logoutId).Result;
            return new LogoutContext()
            {
                PostLogoutRedirectUri = context?.PostLogoutRedirectUri ?? null
            };

        }

        public LoginContext GetLoginContext(string redirectUrl)
        {
            var context = _interactionService.GetAuthorizationContextAsync(redirectUrl).Result;
            return context == null ? null : new LoginContext();
        }


        public string GetUserIdFromClaims(List<Claim> claims)
        {
            var result = "";
            result = claims?.Where(x => x.Type == "sub").FirstOrDefault()?.Value ?? "";
            return result;
        }

        public void Logout()
        {
            _signInManager.SignOutAsync().Wait();
        }
    }

    public class FunctionClaimAuthAttribute : TypeFilterAttribute
    {
        public FunctionClaimAuthAttribute(string functionClaim) : base(typeof(FunctionClaimFilter))
        {
            Arguments = new object[] { functionClaim };
        }
    }

    public class FunctionClaimFilter : IAuthorizationFilter
    {
        readonly string _functionClaim;

        public FunctionClaimFilter(string functionClaim)
        {
            _functionClaim = functionClaim;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // check if user has the corresponding claim
            var hasClaim = context.HttpContext?.User?.Claims?.Any(c => c.Type == CommonConstant.FunctionClaimTypeConstant && c.Value == _functionClaim) ?? false;
            // check if user is the admin
            var username = context.HttpContext?.User?.Claims?.Where(c => c.Type == "name").FirstOrDefault()?.Value ?? "";

            if (!hasClaim && username != CommonConstant.AGSAdminName)
            {
                context.Result = new UnauthorizedResult();
            }

        }
    }


}
