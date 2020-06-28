using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AGSIdentity.Services.AuthService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AGSIdentity.Pages
{
    public class logoutModel : PageModel
    {
        private IAuthService _authService { get; set; }

        public logoutModel(IAuthService authService)
        {
            _authService = authService;
        }

        public void OnGet()
        {
            try
            {
                // logout
                _authService.Logout();

                // if redirect url is provided, then redirect the browser to it. Otherwise, go back to login page
                var redirectUrl = _authService.GetRedirectUrl();
                if (redirectUrl != null) {
                    var clientInfo = _authService.GetClientInfoInAuthoriationRequest(redirectUrl);
                    if (clientInfo != null)
                    {
                        Response.Redirect(redirectUrl);
                    }
                }

            }catch(Exception ex)
            {

            }
        }
    }
}
