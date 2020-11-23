using System;
using AGSIdentity.Models.ViewModels.API.Common;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AGSIdentity.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {
        private ILogger _logger { get; set; }

        public ErrorController(ILogger logger)
        {
            _logger = logger;
        }

        [Route("/error")]
        public IActionResult Error()
        {
            var error = HttpContext.Features.Get<IExceptionHandlerFeature>();

            if (error != null)
            {
                var exception = error.Error;
                var agsException = exception as AGSException;
                // if it is AGSException, return response code 
                if (agsException != null) {
                    return AGSResponseFactory.GetAGSExceptionJsonResult(agsException);
                }else
                {
                    // if it is other exceptions, shows error with http response code 500
                    return Problem(exception.Message);
                }

            }else {
                return Problem();
            }
        }
    }
}
