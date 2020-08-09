using System;
using System.Collections.Generic;
using System.IO;
using AGSCommon.Models.EntityModels.Common;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace AGS.Services.AGS.Localization.Json
{
    public class JsonLocalizationService : ILocalizationService
    {
        private string _localization_folder_path { get; set; }

        private Dictionary<string, Dictionary<string, string>> _localization_resources { get; set; } = new Dictionary<string, Dictionary<string, string>>();

        public JsonLocalizationService(IConfiguration configuration)
        {
            _localization_folder_path = configuration["localization_folder_path"];
            var localization_files_path = Directory.GetFiles(_localization_folder_path);
            if (localization_files_path != null)
            {
                foreach(var localization_file_path in localization_files_path)
                {
                    string localization_file_name = Path.GetFileNameWithoutExtension(localization_file_path);
                    string localization_file_content = File.ReadAllText(localization_file_path);
                    var localization_resource = JsonConvert.DeserializeObject<Dictionary<string, string>>(localization_file_content);
                    if (localization_resource != null)
                    {
                        _localization_resources.Add(localization_file_name, localization_resource);
                    }
                }
            }
        }

        public string GetLocalizedString(string labelKey, string locale)
        {
            if (string.IsNullOrEmpty(labelKey))
            {
                return "";
            }

            if (string.IsNullOrEmpty(locale))
            {
                throw new ArgumentNullException();
            }

            try
            {
                var localizedString = _localization_resources[locale][labelKey];
                if (string.IsNullOrEmpty(localizedString))
                {
                    return labelKey;
                }
                else
                {
                    return localizedString;
                }
                
            }catch(Exception ex)
            {
                return labelKey;
            }
            
        }

        public string GetLocalizedError(string responseCode, string locale = "en-US")
        {
            return GetLocalizedString("ags_error_" + responseCode, locale);
        }
    }
}
