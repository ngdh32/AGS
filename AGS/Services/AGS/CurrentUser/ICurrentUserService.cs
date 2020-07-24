using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace AGS.Services.AGS.CurrentUser
{
    public interface ICurrentUserService
    {
        string GetCurrentUserId();

        string GetAccessToken();

        string GetCurrentLang();

        List<Claim> GetCurrentUserClaims();
    }
}
