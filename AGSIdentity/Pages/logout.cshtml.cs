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

        }

        public void OnPost()
        {
            try
            {
                // logout
                _authService.Logout();

                var logoutId = HttpContext.Request.Query["logoutId"].ToString();
                // to get the post logout redirect url 
                var clientInfo = _authService.GetLogoutContext(logoutId);
                if (clientInfo != null)
                {
                    Response.Redirect(clientInfo.PostLogoutRedirectUri);
                }

            }
            catch(Exception ex)
            {

            }
        }
    }
}
