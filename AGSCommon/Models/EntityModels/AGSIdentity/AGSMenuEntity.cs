using System;

namespace AGSCommon.Models.EntityModels.AGSIdentity
{
    public class AGSMenuEntity : AGSIdentityEntity
    {
        public string Id { get; set; } = AGSCommon.CommonFunctions.GenerateId();

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public int Order { get; set; }

        // the functionclaim that allow users to read the menu option
        public string FunctionClaimId { get; set; }

        public string ParentId { get; set; }

        public AGSMenuEntity()
        {
        }
    }
}
