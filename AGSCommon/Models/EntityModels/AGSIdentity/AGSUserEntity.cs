﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace AGSCommon.Models.EntityModels.AGSIdentity
{
    public class AGSUserEntity : AGSIdentityEntity
    {
        public string Id { get; set; } = AGSCommon.CommonFunctions.GenerateId();

        public string Username { get; set; }

        public string Email { get; set; }

        public string First_Name { get; set; }

        public string Last_Name { get; set; }

        public string Title { get; set; }

        public List<string> GroupIds { get; set; } = new List<string>();

        public AGSUserEntity()
        {
        }
    }
}
