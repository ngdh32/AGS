﻿using System;
namespace AGSDocumentCore.Models.Entities
{
    public abstract class AGSEntity
    {
        public string Id { get; protected set; } = CommonUtility.GenerateId();
    }
}
