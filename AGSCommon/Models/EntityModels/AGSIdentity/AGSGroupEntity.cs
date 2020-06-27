using System;
using System.Collections.Generic;
using System.Linq;

namespace AGSCommon.Models.EntityModels.AGSIdentity
{
    public class AGSGroupEntity : AGSIdentityEntity
    {
        public string Id { get; set; } = AGSCommon.CommonFunctions.GenerateId();

        public string Name { get; set; }

        public List<string> FunctionClaimIds { get; set; } = new List<string>();

        public AGSGroupEntity()
        {
        }
    }
}
