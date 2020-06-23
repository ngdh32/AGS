using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AGSCommon.Models.DataModels.AGSIdentity
{
    public class AGSMenu : AGSIdentityDataModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int Order { get; set; }

        // the functionclaim that allow users to read the menu option
        public AGSFunctionClaim FunctionClaim { get; set; }

        public AGSMenu ParentMenu { get; set; }

    }
}
