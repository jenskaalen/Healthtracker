using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Healthtracker.Web.Config
{

    public class JsonConfiguration : IConfiguration
    {
        public string this[string key] { get => GetSetting(key); set => throw new NotImplementedException(); }

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            //throw new NotImplementedException();
            return new List<IConfigurationSection>();
        }

        public IChangeToken GetReloadToken()
        {
            throw new NotImplementedException();
        }

        public IConfigurationSection GetSection(string key)
        {
            throw new NotImplementedException();
        }

        public string GetSetting(string setting)
        {
            string json = System.IO.File.ReadAllText("config\\settings.json");
            Dictionary<string, string> settings = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            return settings[setting];
        }
    }
}
