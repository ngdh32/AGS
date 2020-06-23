using System;
using System.ComponentModel.DataAnnotations;
using AGSCommon.Models.DataModels.AGSIdentity;

namespace AGSIdentity.Models.EntityModels.EF
{
    public class FunctionClaim
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
