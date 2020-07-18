using System;
namespace AGSCommon.Models.EntityModels.AGSIdentity
{
    public class AGSConfigEntity : AGSIdentityEntity
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public bool isSecure { get; set; }

        public AGSConfigEntity()
        {
        }
    }
}
