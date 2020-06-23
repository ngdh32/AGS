using System;
using System.Collections.Generic;

namespace AGSIdentity.Models.EntityModels
{
    public class AGSGroupEntity
    {
        public string Id { get; set; } = AGSCommon.CommonFunctions.GenerateId();

        public string Name { get; set; }

        public List<string> FunctionClaimIds { get; set; }

        public AGSGroupEntity()
        {
        }
    }
}
