using System;
using System.Security.Claims;
using AGSIdentity.Models.ViewModels.Login;

namespace AGSIdentity.Services.Auth
{
    public interface IAuthService
    {
        bool Login(LoginInputModel loginInputModel);

        void Logout();

        ClaimsPrincipal GetCurrentUser();
    }
}
