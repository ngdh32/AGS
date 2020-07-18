using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace AGS.Controllers
{
    public class AccountController : Controller
    {
        public AccountController()
        {
        }

        public IActionResult SignIn([FromRoute] string scheme)
        {
            scheme = scheme ?? "oidc";
            var redirectUrl = Url.Content("~/");
            return Challenge(
                new AuthenticationProperties { RedirectUri = redirectUrl },
                scheme);
        }

        [HttpGet("logout")]
        public IActionResult SignOut()
        {
            return new SignOutResult(new[] { "oidc", "Cookies" });
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetUserInfo(){
            var claims = new List<string>();
            foreach(var claim in HttpContext.User.Claims){
                claims.Add(claim.Type + ":" + claim.Value);
            }

            return Json(new {
                username = HttpContext.User.Identity.Name,
                claims = claims
            });
        }
    }
}
