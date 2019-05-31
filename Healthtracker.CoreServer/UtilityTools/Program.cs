using MongoDB.Driver;
using Raven.Client.Documents;
using System;
using System.Linq;

namespace UtilityTools
{
    class Program
    {
        static void Main(string[] args)
        {
            //DataConversionJob.ConvertJson();
            //using (var store = GetDocumentStore())
            //{
            //    store.Initialize();

            //    using (var session = store.OpenSession())
            //    {
            //        var log = session.Load<Log>("logs/66");
            //        //var shipper = session.Load<Shippers>("shippers/1-A");
            //        //Console.WriteLine("Shipper #1 : " + shipper.Name + ", Phone: " + shipper.Phone);
            //    }
            //}
        }

        private static DocumentStore GetDocumentStore()
        {
            return new DocumentStore()
            {
                Urls = new string[] { "http://10.0.0.95:8080" },
                Database = "LogDb",
                Conventions =
                    {
                        FindIdentityProperty = prop => prop.Name == "DocumentId"
                    }
            };
        }
    }
}
