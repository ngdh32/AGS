using System;
using System.ComponentModel.DataAnnotations;
using AGSCommon.Models.EntityModels.AGSIdentity;

namespace AGSIdentity.Models.EntityModels.EF
{
    public class EFFunctionClaim
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

    }
}
