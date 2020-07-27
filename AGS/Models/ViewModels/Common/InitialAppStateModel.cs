using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace AGS.Models.ViewModels.Common
{
    public class InitialAppStateModel
    {
        public string AccessToken { get;set; }

        public string Lang { get; set; }

        public string UserId { get; set; }

        public string UserClaimText { get; set; }

        public string Username { get; set; }

        public const string TypeValueSeperator = "%|%";

        public const string EntitySeperator = "%+%";

        //refresh_token later on

        public InitialAppStateModel()
        {
        }

        public InitialAppStateModel(HttpContext httpContext)
        {
            if (httpContext.User != null)
            {
                this.AccessToken = httpContext.GetTokenAsync("access_token").Result;
                this.UserId = httpContext.User?.Claims?.Where(x => x.Type == "sub").FirstOrDefault()?.Value ?? "";
                httpContext.Request.Cookies.TryGetValue(AGSCommon.CommonConstant.AGSConstant.localization_lang_cookie_name, out var lang_cookie);
                this.Lang = lang_cookie;
                UserClaimText = "";
                httpContext.User.Claims.ToList().ForEach(x =>
                {
                    UserClaimText += x.Type + TypeValueSeperator + x.Value + EntitySeperator;
                });
                if (!string.IsNullOrEmpty(UserClaimText) && UserClaimText.Length > 0)
                {
                    UserClaimText = UserClaimText.Substring(0, UserClaimText.Length - EntitySeperator.Length);
                }
                this.Username = httpContext.User?.Claims?.Where(x => x.Type == "name").FirstOrDefault()?.Value ?? "";
            }

        }


        public static List<Claim> UserClaimTextDeserialize(string userClaimText)
        {
            List<Claim> result = new List<Claim>();

            var userClaimEntities = userClaimText.Split(EntitySeperator);
            foreach (var userClaimEntity in userClaimEntities)
            {
                var claimProperties = userClaimEntity.Split(TypeValueSeperator);
                result.Add(new Claim(claimProperties[0], claimProperties[1]));
            }

            return result;
        }
    }
}
