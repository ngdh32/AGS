using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AGSIdentity.Services.AuthService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AGSIdentity.Pages
{
    public class IndexModel : PageModel
    {
        private IAuthService _authService { get; set; }

        [BindProperty]
        public string username { get; set; }

        public IndexModel(IAuthService authService)
        {
            _authService = authService;
        }

        public void OnGet()
        {
            if (!User.Identity.IsAuthenticated){
                Response.Redirect("/login");
                return;
            }

            username = _authService.GetUserIdFromClaims(User.Claims.ToList());

        }
    }
}
