using System;
using System.Security.Claims;
using AGSIdentity.Models.ViewModels.Login;
using IdentityServer4.Models;

namespace AGSIdentity.Services.AuthService
{
    public interface IAuthService
    {
        bool Login(LoginInputModel loginInputModel);

        AuthorizationRequest GetClientInfoInAuthoriationRequest(string redirectUrl);

        string GetRedirectUrl();

        void Logout();

        ClaimsPrincipal GetCurrentUser();
    }
}
