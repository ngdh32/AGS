using System;
using AGSCommon.Models.EntityModels.Common;
using Microsoft.AspNetCore.Mvc;

namespace AGSIdentity
{
    public static class AGSResponseFactory
    {
        public static JsonResult GetAGSExceptionJsonResult(AGSException exception)
        {
            return new JsonResult(new AGSResponse(exception.responseCode));
        }

        public static JsonResult GetAGSResponseJsonResult(object data = null)
        {
            return new JsonResult(new AGSResponse(AGSResponse.ResponseCodeEnum.Done, data));
        }
    }
}
