using System;
using System.Collections.Generic;

namespace AGSDocumentCore.Models.DTOs.Queries
{
    public class AGSUser
    {
        public string UserId { get; init; }
        public string Username { get; init; }
        public List<AGSDepartment> Departments { get; init; }
    }

    public class AGSDepartment
    {
        public string DepartmentId { get; init; }
        public string DepartmentName { get; init; }
    }
}
