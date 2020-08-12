using System;
using AGSCommon.Models.EntityModels.Common;
using Microsoft.AspNetCore.Components;

namespace AGS.Services.AGS
{
    public static class CommonFunctions
    {
        public static void HandleAGSResponseError(AGSResponse.ResponseCodeEnum responseCode, NavigationManager navigationManager, ref string errorMessageField)
        {
            if (responseCode == AGSResponse.ResponseCodeEnum.TokenExpiredError)
            {
                navigationManager.NavigateTo("/logout", true);
            }
            else
            {
                errorMessageField = "ags_error_" +  responseCode.ToString();
            }
        }
    }
}
