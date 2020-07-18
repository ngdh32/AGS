using System;
using System.ComponentModel.DataAnnotations;

namespace AGSIdentity.Models.EntityModels.EF
{
    public class EFConfigValue
    {
        [Key]
        public string Key { get; set; }
        [Required]
        public string Value { get; set; }
        [Required]
        public bool IsSecure { get; set; }

        public EFConfigValue()
        {
        }
    }
}
