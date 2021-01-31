using System;
using System.Collections.Generic;
using System.Security.Claims;
using AGSIdentity.Models.ViewModels.Pages.Login;
using Microsoft.AspNetCore.Mvc;

namespace AGSIdentity.Services.AuthService
{
    public interface IAuthService
    {
        bool Login(LoginInputModel loginInputModel);

        LoginContext GetLoginContext(string redirectUrl);

        LogoutContext GetLogoutContext(string logoutId);

        void Logout();

        string GetUserIdFromClaims(List<Claim> claims);
    }
}
