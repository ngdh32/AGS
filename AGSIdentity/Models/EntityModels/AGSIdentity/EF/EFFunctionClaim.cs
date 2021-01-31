using System;
using System.ComponentModel.DataAnnotations;

namespace AGSIdentity.Models.EntityModels.AGSIdentity.EF
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
