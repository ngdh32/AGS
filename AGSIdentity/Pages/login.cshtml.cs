using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AGSIdentity.Models.ViewModels.Login;
using AGSIdentity.Services.Auth;
using AGSIdentity.Services.ExceptionFactory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;

namespace AGSIdentity.Pages
{
    public class loginModel : PageModel
    {
        private IAuthService _authService { get; set; }
        private IExceptionFactory _exceptionFactory { get; set; }
        

        [BindProperty]
        public LoginInputModel loginInputModel { get; set; }
        [BindProperty]
        public string errorMessage { get; set; }

        public loginModel(IAuthService authService, IExceptionFactory exceptionFactory)
        {
            _authService = authService;
            _exceptionFactory = exceptionFactory;
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
                    throw _exceptionFactory.GetErrorByCode(ErrorCodeEnum.UsernameOrPasswordError);
                }
                else
                {   
                    var redirectUrl = _authService.GetRedirectUrl();

                    // check if the redirect url is valid for security issue
                    if (_authService.CheckIfInLoginRequest(redirectUrl))
                    {
                        Console.WriteLine("redirect url valid!");
                        // redirect the request to the identity server service and continue the process
                        Response.Redirect(redirectUrl);
                    }
                    else
                    {
                        throw _exceptionFactory.GetErrorByCode(ErrorCodeEnum.RedirectUrlError);
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
