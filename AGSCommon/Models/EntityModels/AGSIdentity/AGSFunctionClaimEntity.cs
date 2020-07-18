using System;

namespace AGSCommon.Models.EntityModels.AGSIdentity
{
    public class AGSFunctionClaimEntity : AGSIdentityEntity
    {
        public string Id { get; set; } = AGSCommon.CommonFunctions.GenerateId();

        public string Name { get; set; }

        public string Description { get; set; }

        public AGSFunctionClaimEntity()
        {
        }

    }
}
