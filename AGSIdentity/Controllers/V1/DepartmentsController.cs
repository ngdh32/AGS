using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AGSIdentity.Helpers;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AGSIdentity.Controllers.V1
{
    public class DepartmentsController : AGSBaseController
    {
        private readonly DepartmentsHelepr _departmentsHelepr;

        public DepartmentsController(DepartmentsHelepr departmentsHelepr)
        {
            _departmentsHelepr = departmentsHelepr;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return Ok();
        }
    }
}
