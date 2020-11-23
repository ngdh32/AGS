using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AGSIdentity.Models.ViewModels.Pages.Login;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using AGSIdentity.Services.AuthService;

namespace AGSIdentity.Pages
{
    public class loginModel : PageModel
    {
        private IAuthService _authService { get; set; }
        

        [BindProperty]
        public LoginInputModel loginInputModel { get; set; }
        [BindProperty]
        public string errorMessage { get; set; }

        public loginModel(IAuthService authService)
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
                var loginResult = _authService.Login(loginInputModel);
                if (!loginResult)
                {
                    errorMessage = "Username or password invalid";
                }
                else
                {
                    var redirectUrl = HttpContext.Request.Query["ReturnUrl"].ToString();
                    redirectUrl = redirectUrl == null ? "" : WebUtility.UrlDecode(redirectUrl);
                    // check if redirectUrl is generated from IS4
                    var loginContext = _authService.GetLoginContext(redirectUrl);


                    if (loginContext == null)
                    {
                        Console.WriteLine("redirect url valid!");
                        // redirect the request to the identity server service and continue the process
                        Response.Redirect(redirectUrl);
                    }
                    else
                    {
                        errorMessage = "It is not a valid login request";
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }

        
    }
}
