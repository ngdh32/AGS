using System;
using System.Collections.Generic;

namespace AGSCommon.Models.DataModels.AGSIdentity
{
    public class AGSGroup : AGSIdentityDataModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public List<AGSFunctionClaim> FunctionClaims { get; set; } = new List<AGSFunctionClaim>();
    }
}
