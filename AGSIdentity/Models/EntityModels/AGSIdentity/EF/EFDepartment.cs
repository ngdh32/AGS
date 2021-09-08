using System;
using System.ComponentModel.DataAnnotations;

namespace AGSIdentity.Models.EntityModels.AGSIdentity.EF
{
    public class EFDepartment
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }

        public string ParentDepartmentId { get; set; }

        public string HeadUserId { get; set; }
    }
}
