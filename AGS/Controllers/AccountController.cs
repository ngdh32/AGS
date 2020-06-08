using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

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
    }
}
