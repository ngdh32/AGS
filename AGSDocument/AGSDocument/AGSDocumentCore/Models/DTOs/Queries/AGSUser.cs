using System;
using System.Collections.Generic;

namespace AGSDocumentCore.Models.DTOs.Queries
{
    public record AGSUser(string userId, string username, List<AGSDepartment> departments);

    public record AGSDepartment(string departmentId, string departmentName);
}
