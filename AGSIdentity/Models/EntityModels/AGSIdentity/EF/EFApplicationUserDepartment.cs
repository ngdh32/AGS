using System;
namespace AGSIdentity.Models.EntityModels.AGSIdentity.EF
{
    public class EFApplicationUserDepartment
    {
        public string UserId { get; set; }
        public EFApplicationUser User { get; set; }
        public string DepartmentId { get; set; }
        public EFDepartment Department { get; set; }
    }
}
