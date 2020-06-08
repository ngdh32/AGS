using System;
using System.Security.Claims;
using AGSIdentity.Models;
using AGSIdentity.Models.ViewModels.Login;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace AGSIdentity.Services.Auth.Identity
{
    public class IdentityAuthService : IAuthService
    {
        private IHttpContextAccessor _httpContextAccessor { get; set; }
        private SignInManager<ApplicationUser> _signInManager { get; set; }
        private UserManager<ApplicationUser> _userManager { get; set; }
        private IIdentityServerInteractionService _interactionService { get; set; }

        public IdentityAuthService(IHttpContextAccessor httpContextAccessor, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IIdentityServerInteractionService interactionService)
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

        public string GetRedirectUrl(){
            var redirectUrl =  _httpContextAccessor.HttpContext.Request.Query["ReturnUrl"].ToString();
            redirectUrl = WebUtility.UrlDecode(redirectUrl);
            return redirectUrl;
        }

        public bool CheckIfInLoginRequest(string redirectUrl)
        {
            var context = _interactionService.GetAuthorizationContextAsync(redirectUrl).Result;
            if (context != null)
            {
                return true;
            }else
            {
                return false;
            }
        }

        public void Logout()
        {
            throw new NotImplementedException();
        }
    }
}
