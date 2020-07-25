using System;
using System.Collections.Generic;
using System.Security.Claims;
using AGS.Models.ViewModels.Common;
using Microsoft.AspNetCore.Http;

namespace AGS.Services.AGS.CurrentUser
{
    public interface ICurrentUserService
    {
        void SetupInitialState(string accessToken, string userId, string currentLang, List<Claim> userClaims);

        string GetCurrentUserId();

        string GetAccessToken();

        string GetCurrentLang();

        List<Claim> GetCurrentUserClaims();
    }
}
