using System;
using System.Security.Claims;

namespace AGSCommon.Services.GetCurrentUser
{
    public interface IGetCurrentUserService
    {
        ClaimsPrincipal GetCurrentUser();
    }
}
