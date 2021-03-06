﻿using System;
namespace AGSIdentity.Models.ViewModels.API.Common
{
    public class AGSResponse
    {

        public ResponseCodeEnum Code { get; set; }

        public object Data { get; set; }

        public AGSResponse()
        {

        }

        public AGSResponse(ResponseCodeEnum responseCode)
        {
            this.Code = responseCode;
            this.Data = null;
        }

        public AGSResponse(ResponseCodeEnum responseCode, object Data)
        {
            this.Code = responseCode;
            this.Data = Data;
        }

        public enum ResponseCodeEnum
        {
            Done = 10000,
            UsernameOrPasswordError = 90000,
            RedirectUrlError = 90001,
            ModelNotFound = 90002,
            DefaultPasswordNotFound = 90003,
            NoPermissionError = 90004,
            TokenExpiredError = 90005,
            UsernameDuplicate = 90006,

            UnknownError = 99999
        }
    }
}
