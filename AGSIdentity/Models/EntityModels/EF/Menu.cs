using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AGSCommon.Models.DataModels.AGSIdentity;

namespace AGSIdentity.Models.EntityModels.EF
{
    public class Menu
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Name { get; set; }

        [Required]
        public int Order { get; set; }

        // the functionclaim that allow users to read the menu option
        [ForeignKey("FunctionClaim")]
        public string FunctionClaimId { get; set; }

        [ForeignKey("Menu")]
        public string ParentId { get; set; } = null;

    }
}
