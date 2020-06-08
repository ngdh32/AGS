using System;
using System.ComponentModel.DataAnnotations;

namespace AGSCommon.Models.DataModels.AGSIdentity
{
    public class AGSFunctionClaim
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
