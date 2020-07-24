using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace AGS.Services.AGS.CurrentUser.HttpContext
{
    public class HttpContextCurrentUserService : ICurrentUserService
    {
        private IHttpContextAccessor _httpContextAccessor { get; set; }

        public HttpContextCurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetAccessToken()
        {
            return _httpContextAccessor.HttpContext.GetTokenAsync("access_token").Result;
        }

        public string GetCurrentUserId()
        {
            var result = "";
            result = _httpContextAccessor?.HttpContext?.User?.Claims?.Where(x => x.Type == "sub").FirstOrDefault()?.Value ?? "";
            return result;
        }

        public string GetCurrentLang()
        {
            _httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(AGSCommon.CommonConstant.AGSConstant.localization_lang_cookie_name, out var lang_cookie);
            return lang_cookie;
        }

        public List<Claim> GetCurrentUserClaims()
        {
            return _httpContextAccessor.HttpContext.User.Claims.ToList();
        }
    }
}
