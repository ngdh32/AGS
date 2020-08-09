using System;
using AGSCommon.Models.EntityModels.Common;

namespace AGS.Services.AGS.Localization
{
    public interface ILocalizationService
    {
        string GetLocalizedString(string labelKey, string locale = AGSCommon.CommonConstant.AGSConstant.localization_lang_enus);

        string GetLocalizedError(string responseCode, string locale = AGSCommon.CommonConstant.AGSConstant.localization_lang_enus);
    }
}
