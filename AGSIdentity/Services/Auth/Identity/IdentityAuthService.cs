using System;
using System.Security.Claims;
using AGSIdentity.Models;
using AGSIdentity.Models.ViewModels.Login;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace AGSIdentity.Services.Auth.Identity
{
    public class IdentityAuthService : IAuthService
    {
        private IHttpContextAccessor _httpContextAccessor { get; set; }
        private SignInManager<ApplicationUser> _signInManager { get; set; }
        private UserManager<ApplicationUser> _userManager { get; set; }

        public IdentityAuthService(IHttpContextAccessor httpContextAccessor, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _signInManager = signInManager;
            _userManager = userManager;
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
                    var result = _signInManager.CheckPasswordSignInAsync(selected, loginInputModel.password, false).Result;
                    isLoginSuccessful = result.Succeeded;
                }
                else
                {
                    isLoginSuccessful = false;
                }
            }

            return isLoginSuccessful;

        }

        public void Logout()
        {
            throw new NotImplementedException();
        }
    }
}
