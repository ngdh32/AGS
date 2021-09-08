using System;
namespace AGSIdentity.Models.EntityModels.AGSIdentity
{
    public class AGSDepartmentEntity : AGSBaseEntity
    {
        public string Name { get; set; }

        public string HeadUserId { get; set; }

        public string ParentDepartmentId { get; set; }  

        public AGSDepartmentEntity()
        {
        }
    }
}
