using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AGSIdentity.Models.ViewModels.Login;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using AGSIdentity.Services.AuthService;
using AGSIdentity.Services.ExceptionService;

namespace AGSIdentity.Pages
{
    public class loginModel : PageModel
    {
        private IAuthService _authService { get; set; }
        private IExceptionService _exceptionService { get; set; }
        

        [BindProperty]
        public LoginInputModel loginInputModel { get; set; }
        [BindProperty]
        public string errorMessage { get; set; }

        public loginModel(IAuthService authService, IExceptionService exceptionService)
        {
            _authService = authService;
            _exceptionService = exceptionService;
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
                    throw _exceptionService.GetErrorByCode(ErrorCodeEnum.UsernameOrPasswordError);
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
                        throw _exceptionService.GetErrorByCode(ErrorCodeEnum.RedirectUrlError);
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
