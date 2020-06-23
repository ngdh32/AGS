using System;
using System.ComponentModel.DataAnnotations;

namespace AGSCommon.Models.DataModels.AGSIdentity
{
    public class  AGSFunctionClaim : AGSIdentityDataModel
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
