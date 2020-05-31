using System;
using System.ComponentModel.DataAnnotations;

namespace AGSIdentity.Models.DataModels
{
    public class FunctionClaim
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
