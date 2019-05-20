
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Json.Net;
using LiteDB;
using Newtonsoft.Json;

namespace UtilityTools
{
    public class DataConversionJob
    {
        public void ConvertJson()
        {
            using (StreamReader reader = new StreamReader("logdata.json"))
            {
                string jsonData = reader.ReadToEnd();
                var logs = JsonConvert.DeserializeObject<List<Log>>(jsonData);

                foreach (var log in logs)
                {
                    log.UserId = "ja.kaalen@gmail.com";
                    Console.WriteLine(log.Id);
                }


                using (var db = new LiteDatabase(@"logdata.db"))
                {
                    var col = db.GetCollection<Log>("logs");
                    //col.InsertBulk(logs);
                    var all = col.FindAll().ToList();
                    Console.WriteLine($"counti s {all.Count()}");
                }

                Console.ReadLine();
            }
        }
    }
}
