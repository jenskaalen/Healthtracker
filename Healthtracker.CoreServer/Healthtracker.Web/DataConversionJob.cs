using Healthtracker.Web.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Healthtracker.Web
{
    public class DataConversionJob
    {
        public void ConvertJson()
        {
            using (StreamReader reader = new StreamReader("Data\\logdata.json"))
            {
                string jsonData = reader.ReadToEnd();
                var logs = JsonConvert.DeserializeObject<List<Log>>(jsonData);

                foreach (var log in logs)
                {
                    Console.WriteLine(log.Id);
                }
            }
        }
    }
}
