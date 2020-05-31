using System;
using System.Collections.Generic;
using System.IO;
using AGSIdentity.Models.ExceptionModels;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace AGSIdentity.Services.ExceptionFactory.Json
{
    public class JsonExceptionFactory : IExceptionFactory
    {
        private IConfiguration _configuration { get; set; }
        private Dictionary<ErrorCodeEnum, string> _erors { get; set; }

        public JsonExceptionFactory(IConfiguration configuration)
        {
            _configuration = configuration;

            // only loaded once when the app is initialized
            var errorCodePath = _configuration["error_code_file_path"];
            _erors = JsonConvert.DeserializeObject<Dictionary<ErrorCodeEnum, string>>(File.ReadAllText(errorCodePath));
        }

        public AGSException GetErrorByCode(ErrorCodeEnum errorCode)
        {
            string errorMessage = _erors[errorCode];
            return new AGSException(errorMessage);
        }
    }
}
