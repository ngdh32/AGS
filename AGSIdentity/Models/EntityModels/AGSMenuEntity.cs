using System;
namespace AGSIdentity.Models.EntityModels
{
    public class AGSMenuEntity
    {
        public string Id { get; set; } = AGSCommon.CommonFunctions.GenerateId();

        public string Name { get; set; }

        public int Order { get; set; }

        // the functionclaim that allow users to read the menu option
        public string FunctionClaimId { get; set; }

        public string ParentId { get; set; } = null;

        public AGSMenuEntity()
        {
        }
    }
}
