using System;
using System.Security.Claims;
using AGSIdentity.Models.ViewModels.Login;

namespace AGSIdentity.Services.AuthService
{
    public interface IAuthService
    {
        bool Login(LoginInputModel loginInputModel);

        bool CheckIfInLoginRequest(string redirectUrl);

        string GetRedirectUrl();

        void Logout();

        ClaimsPrincipal GetCurrentUser();
    }
}
