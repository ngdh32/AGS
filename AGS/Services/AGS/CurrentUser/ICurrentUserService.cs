using System;
using System.Collections.Generic;
using System.Security.Claims;
using AGS.Models.ViewModels.Common;
using Microsoft.AspNetCore.Http;

namespace AGS.Services.AGS.CurrentUser
{
    /// <summary>
    /// Get the required proeprties from _host.cshtml
    /// </summary>
    public interface ICurrentUserService
    {
        void SetupInitialState(string username, string accessToken, string userId, string currentLang, List<Claim> userClaims);

        string GetCurrentUserId();

        string GetAccessToken();

        string GetCurrentLang();

        string GetCurrentUsername();

        List<Claim> GetCurrentUserClaims();
    }
}
