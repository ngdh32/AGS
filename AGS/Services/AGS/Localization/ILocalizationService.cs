using System;
namespace AGS.Services.AGS.Localization
{
    public interface ILocalizationService
    {
        string GetLocalizedString(string labelKey, string locale = AGSCommon.CommonConstant.AGSConstant.localization_lang_enus);
    }
}
