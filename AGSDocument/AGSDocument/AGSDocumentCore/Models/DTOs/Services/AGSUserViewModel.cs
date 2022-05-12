using System;
using System.Collections.Generic;

namespace AGSDocumentCore.Models.DTOs.Services
{
    public class AGSUserViewModel
    {
        public string UserId { get; init; }
        public string Username { get; init; }
        public List<AGSDepartmentViewModel> Departments { get; init; }
    }

    public class AGSDepartmentViewModel
    {
        public string DepartmentId { get; init; }
        public string DepartmentName { get; init; }
    }
}
