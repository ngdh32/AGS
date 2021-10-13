using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AGSIdentity.Attributes;
using AGSIdentity.Helpers;
using AGSIdentity.Models.EntityModels.AGSIdentity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AGSIdentity.Controllers.V1
{
    public class DepartmentsController : V1BaseController
    {
        private readonly DepartmentsHelepr _departmentsHelepr;

        public DepartmentsController(DepartmentsHelepr departmentsHelepr)
        {
            _departmentsHelepr = departmentsHelepr;
        }

        /// <summary>
        /// Return all the departments
        /// </summary>
        /// <response code="401">if no token of invalid token is passed</response>          
        /// <response code="403">if the logged user doesn't have the correct departments</response>  
        [HttpGet]
        [AGSResultActionFilter]
        [Authorize(Policy = CommonConstant.AGSDepartmentReadClaimConstant)]
        public List<AGSDepartmentEntity> Get()
        {
            var result = _departmentsHelepr.GetAllDepartments();
            return result;
        }


        /// <summary>
        /// Return the specified departments
        /// </summary>
        /// <param name="id"></param>
        /// <response code="401">if no token of invalid token is passed</response>          
        /// <response code="403">if the logged user doesn't have the correct departments</response>  
        [HttpGet("{id}")]
        [AGSResultActionFilter]
        [Authorize(Policy = CommonConstant.AGSDepartmentReadClaimConstant)]
        public AGSDepartmentEntity Get(string id)
        {
            var result = _departmentsHelepr.GetDepartmentById(id);
            return result;
        }

        /// <summary>
        /// Return the specified departments
        /// </summary>
        /// <param name="id"></param>
        /// <response code="401">if no token of invalid token is passed</response>          
        /// <response code="403">if the logged user doesn't have the correct departments</response>  
        [HttpGet("{id}/users")]
        [AGSResultActionFilter]
        [Authorize(Policy = CommonConstant.AGSDepartmentReadClaimConstant)]
        public List<AGSUserEntity> GetAllUsersByDepartmentId(string id)
        {
            var result = _departmentsHelepr.GetAllUsersByDepartmentId(id);
            return result;
        }


        /// <summary>
        /// Create a function claim. Only users with specified claim are allowed 
        /// </summary>
        /// <param name="Department"></param>
        /// <response code="401">if no token of invalid token is passed</response>          
        /// <response code="403">if the logged user doesn't have the correct departments</response>  
        [HttpPost]
        [AGSResultActionFilter]
        [Authorize(Policy = CommonConstant.AGSDepartmentEditClaimConstant)]
        public string Post([FromBody] AGSDepartmentEntity Department)
        {
            var result = _departmentsHelepr.CreateDepartment(Department);
            return result;
        }

        /// <summary>
        /// Update a function claim. Only users with specified claim are allowed
        /// </summary>
        /// <param name="Department"></param>
        /// <param name="id"></param>
        /// <response code="401">if no token of invalid token is passed</response>          
        /// <response code="403">if the logged user doesn't have the correct departments</response>  
        [HttpPut]
        [AGSResultActionFilter]
        [Authorize(Policy = CommonConstant.AGSDepartmentEditClaimConstant)]
        public int Put([FromBody] AGSDepartmentEntity Department)
        {
            var result = _departmentsHelepr.UpdateDepartment(Department);
            return result;
        }


        /// <summary>
        /// Delete a function claim. Only users with specified claim are allowed
        /// </summary>
        /// <param name="id"></param>
        /// <response code="401">if no token of invalid token is passed</response>          
        /// <response code="403">if the logged user doesn't have the correct departments</response>  
        [HttpDelete("{id}")]
        [AGSResultActionFilter]
        [Authorize(Policy = CommonConstant.AGSDepartmentEditClaimConstant)]
        public bool Delete(string id)
        {
            _departmentsHelepr.DeleteDepartment(id);
            return true;
        }
    }
}
