using System;
using Microsoft.AspNetCore.Mvc.Filters;
using AGSIdentity.Models.ViewModels.API.Common;
using Microsoft.AspNetCore.Mvc;

namespace AGSIdentity.Attributes
{
    public class AGSResultActionFilter : ResultFilterAttribute
    {
        public AGSResultActionFilter()
        {

        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var result = context.Result;
            // check if the result is JsonResult
            if (result is ObjectResult resultObj)
            {
                // change it to the AGSResponse
                var finalResult = new AGSResponse(AGSResponse.ResponseCodeEnum.Done, resultObj.Value);
                context.Result = new JsonResult(finalResult);
            }

            // continue the process
            base.OnResultExecuting(context);
        }

    }

}