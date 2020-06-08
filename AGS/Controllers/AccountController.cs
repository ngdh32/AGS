using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

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
