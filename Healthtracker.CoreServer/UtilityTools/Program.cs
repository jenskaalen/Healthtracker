using Healthtracker.Web.Model;
using MongoDB.Driver;
using Raven.Client.Documents;
using System;
using System.Collections.Generic;
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

            using (var store = GetDocumentStore())
            {
                store.Initialize();

                using (var session = store.OpenSession())
                {
                    var logs = session.Query<Log>();

                    foreach (Log log in logs)
                    {
                        if (log.Activity == null)
                            continue;

                        var activities = log.Activity.Split(new char[] { ',', '.' }, StringSplitOptions.None).ToList().Select(x => x.Trim()).ToList();
                        //activities.ForEach(x => x.Trim());

                        //activities.ToList().ForEach(x => x.Trim());

                        if (log.Activities == null)
                            log.Activities = new List<LogActivity>();

                        activities.ToList().ForEach(act =>
                        {
                            if (!log.Activities.Select(activ => activ.Name).Contains(act))
                                log.Activities.Add(new LogActivity{ Name = act, IntegrationSource = IntegrationSource.User });
                        });
                    }

                    session.SaveChanges();



                    //IEnumerable<LogActivity> result = session.Query<Log>().Where(x => x.UserId == userId && x.Activities != null && x.Activities.Count > 0).ToList().SelectMany(x => x.Activities)
                    //    .GroupBy(x => x.Name)
                    //  .OrderByDescending(g => g.Count())
                    //  .SelectMany(g => g).ToList();

                    //return result.Select(x => x.Name).Distinct().ToList();
                }
            }
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
