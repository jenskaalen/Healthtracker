
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Json.Net;
using LiteDB;
using Newtonsoft.Json;
using Raven.Client.Documents;

namespace UtilityTools
{
    public class DataConversionJob
    {
        public static void ConvertJson()
        {
            //using (StreamReader reader = new StreamReader(@"D:\LiteDb\logdata.json"))
            //{
            //    //string jsonData = reader.ReadToEnd();
            //    //var logs = JsonConvert.DeserializeObject<List<Log>>(jsonData);

            //    //foreach (var log in logs)
            //    //{
            //    //    log.UserId = "ja.kaalen@gmail.com";
            //    //    Console.WriteLine(log.Id);
            //    //}


            //}


            using (var db = new LiteDatabase(@"D:\LiteDb\logdata.db"))
            {
                var col = db.GetCollection<Log>("logs");
                //col.InsertBulk(logs);
                var all = col.FindAll().ToList();

                using (var store = new DocumentStore
                {
                    Urls = new string[] { "http://10.0.0.95:8080" },
                    Database = "LogDb", Conventions =
                    {
                        FindIdentityProperty = prop => prop.Name == "DocumentId"
                    }
                })
                {
                    store.Initialize();

                    using (var session = store.OpenSession())
                    {

                        foreach (Log log in all)
                        {
                            log.DocumentId = $"logs/{log.Id}";
                            session.Store(log);

                        }
                        session.SaveChanges();
                        //var shipper = session.Load<Shippers>("shippers/1-A");
                        //Console.WriteLine("Shipper #1 : " + shipper.Name + ", Phone: " + shipper.Phone);
                    }
                }



                Console.WriteLine($"counti s {all.Count()}");
            }

            Console.ReadLine();
        }
    }
}
