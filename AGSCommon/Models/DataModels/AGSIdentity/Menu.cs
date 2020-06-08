using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AGSCommon.Models.DataModels.AGSIdentity
{
    public class AGSMenu
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Order { get; set; }

        public AGSFunctionClaim functionClaim { get; set; }

        public AGSMenu Parent { get; set; }

    }
}
