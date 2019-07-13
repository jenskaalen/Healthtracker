using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Healthtracker.Web.Model
{
    public class LogActivity
    {
        public DateTime Date { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public IntegrationSource IntegrationSource { get; set; }
        public string Name { get; set; }
        public string Data { get; set; }
    }

    public enum IntegrationSource
    {
        User,
        Fitbit
    }
}
