using System;
using System.Security.Claims;
using AGSIdentity.Models;
using AGSIdentity.Models.ViewModels.Login;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Net;
using AGSIdentity.Models.EntityModels.EF;
using IdentityServer4.Models;

namespace AGSIdentity.Services.AuthService.Identity
{
    public class IdentityAuthService : IAuthService
    {
        private IHttpContextAccessor _httpContextAccessor { get; set; }
        private SignInManager<EFApplicationUser> _signInManager { get; set; }
        private UserManager<EFApplicationUser> _userManager { get; set; }
        private IIdentityServerInteractionService _interactionService { get; set; }

        public IdentityAuthService(IHttpContextAccessor httpContextAccessor, SignInManager<EFApplicationUser> signInManager, UserManager<EFApplicationUser> userManager, IIdentityServerInteractionService interactionService)
        {
            _httpContextAccessor = httpContextAccessor;
            _signInManager = signInManager;
            _userManager = userManager;
            _interactionService = interactionService;
        }

        public ClaimsPrincipal GetCurrentUser()
        {
            return _httpContextAccessor.HttpContext.User;
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

        public LogoutRequest GetLogoutContext()
        {
            var logoutid = _httpContextAccessor.HttpContext.Request.Query["logoutId"].ToString();
            if (string.IsNullOrEmpty(logoutid))
            {
                logoutid = WebUtility.UrlDecode(logoutid);
            }

            var context = _interactionService.GetLogoutContextAsync(logoutid).Result;
            return context;
        }

        public string GetRedriectUrl()
        {
            var redirectUrl = _httpContextAccessor.HttpContext.Request.Query["ReturnUrl"].ToString();
            if (string.IsNullOrEmpty(redirectUrl))
            {
                redirectUrl = WebUtility.UrlDecode(redirectUrl);
            }

            var context = _interactionService.GetAuthorizationContextAsync(redirectUrl).Result;
            if (context != null)
            {
                return redirectUrl;
            }
            else
            {
                return null;
            }
            
        }

        public void Logout()
        {
            _signInManager.SignOutAsync().Wait();
        }
    }
}
