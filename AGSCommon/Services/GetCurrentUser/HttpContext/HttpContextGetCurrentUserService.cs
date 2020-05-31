using System;
using System.Security.Claims;
using Microsoft

namespace AGSCommon.Services.GetCurrentUser.HttpContext
{
    public class HttpContextGetCurrentUserService
    {
        private IHttpContextAccessor _httpContextAccessor { get; set; }

        public HttpContextGetCurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ClaimsPrincipal GetCurrentUser()
        {
            return _httpContextAccessor.HttpContext.User;
        }
    }
}
