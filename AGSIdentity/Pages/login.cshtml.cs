using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AGSIdentity.Models.ViewModels.Login;
using AGSIdentity.Services.Auth;
using AGSIdentity.Services.ExceptionFactory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AGSIdentity.Pages
{
    public class loginModel : PageModel
    {
        private IAuthService _authService { get; set; }
        private IExceptionFactory _exceptionFactory { get; set; }

        protected string errorMessage { get; set; }

        [BindProperty]
        public LoginInputModel loginInputModel { get; set; }

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
                    errorMessage = "Success!";
                }
            }
            catch(Exception ex)
            {
                errorMessage = ex.Message;
            }
        }
    }
}
