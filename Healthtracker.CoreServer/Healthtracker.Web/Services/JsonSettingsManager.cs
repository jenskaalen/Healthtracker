using Newtonsoft.Json;
using System.Collections.Generic;

namespace Healthtracker.Web.Services
{
    public class JsonSettingsManager : ISettingsManager
    {
        public string GetSetting(AuthSetting setting)
        {
            string json = System.IO.File.ReadAllText("config\\settings.json");
            Dictionary<string, string> settings = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            return settings[setting.ToString()];
        }
    }
}
