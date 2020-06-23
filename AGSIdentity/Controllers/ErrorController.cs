using System;
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
            return Problem();
        }
    }
}
