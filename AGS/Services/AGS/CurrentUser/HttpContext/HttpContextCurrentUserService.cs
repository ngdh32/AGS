using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AGS.Models.ViewModels.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;

namespace AGS.Services.AGS.CurrentUser.HttpContext
{
    public class HttpContextCurrentUserService : ICurrentUserService
    {
        private string AccessToken { get; set; }
        private string UserId { get; set; }
        private string Lang { get; set; }
        private List<Claim> UserClaims { get; set; }

        public HttpContextCurrentUserService()
        {
        }

        public string GetAccessToken()
        {
            return AccessToken;
        }

        public string GetCurrentUserId()
        {
            return UserId;
        }

        public string GetCurrentLang()
        {
            return Lang;
        }

        public List<Claim> GetCurrentUserClaims()
        {
            return UserClaims;
        }

        public void SetupInitialState(string accessToken, string userId, string currentLang, List<Claim> userClaims)
        {
            AccessToken = accessToken;
            UserClaims = userClaims;
            UserId = userId;
            Lang = currentLang;
        }
    }
}
