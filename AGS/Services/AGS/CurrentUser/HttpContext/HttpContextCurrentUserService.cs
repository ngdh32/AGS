using System;
using System.Linq;
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

        public string GetCurrentUserId()
        {
            var result = "";
            result = _httpContextAccessor?.HttpContext?.User?.Claims?.Where(x => x.Type == "sub").FirstOrDefault()?.Value ?? "";
            return result;
        }
    }
}
