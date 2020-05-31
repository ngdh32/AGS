using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AGSIdentity.Models.DataModels
{
    public class Menu
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Order { get; set; }

        public FunctionClaim functionClaim { get; set; }

        public Menu Parent { get; set; }

    }
}
