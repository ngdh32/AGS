using System;
namespace AGSIdentity.Models.EntityModels
{
    public class AGSFunctionClaimEntity
    {
        public string Id { get; set; } = AGSCommon.CommonFunctions.GenerateId();

        public string Name { get; set; }

        public AGSFunctionClaimEntity()
        {
        }
    }
}
