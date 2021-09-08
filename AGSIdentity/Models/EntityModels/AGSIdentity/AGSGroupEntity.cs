using System;
using System.Collections.Generic;
using System.Linq;

namespace AGSIdentity.Models.EntityModels.AGSIdentity
{
    public class AGSGroupEntity : AGSBaseEntity
    {
        public string Name { get; set; }

        public List<string> FunctionClaimIds { get; set; } = new List<string>();

        public AGSGroupEntity()
        {
        }
    }
}
