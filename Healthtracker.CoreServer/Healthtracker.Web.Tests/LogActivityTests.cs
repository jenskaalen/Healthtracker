using Healthtracker.Web.Model;
using Healthtracker.Web.Services;
using NUnit.Framework;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.TestDriver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Healthtracker.Web.Tests
{
    public class LogActivityTests: RavenTestDriver
    {
        //[SetUp]
        //void Setup()
        //{

        //}
        protected override void PreInitialize(IDocumentStore documentStore)
        {
            documentStore.Conventions.MaxNumberOfRequestsPerSession = 50;
            documentStore.Conventions.FindIdentityProperty = prop => prop.Name == "DocumentId";
        }


        [Test]
        public void TestIt()
        {
            ConfigureServer(new TestServerOptions
            {
                DataDirectory = "C:\\temp",
                FrameworkVersion = "2.2.6"
            });

            using (var store = GetDocumentStore())
            {
                var actSugService = new ActivitySuggestionsService(store);
                string userId = "user123";

                var activity1 = new LogActivity()
                {
                    Name = "boxing"
                };

                var activity2 = new LogActivity()
                {
                    Name = "fishing"
                };

                //store.ExecuteIndex(new TestDocumentByName());
                using (var session = store.OpenSession())
                {
                    var log1 = new Log { DocumentId = "logs/1341-A", UserId = userId, Activities = new List<LogActivity> { activity1 } };
                    var log2 = new Log { DocumentId = "logs/1231-A", UserId = userId, Activities = new List<LogActivity> { activity2 } };
                    var log3 = new Log { DocumentId = "logs/1255-A", UserId = userId, Activities = new List<LogActivity> { activity1 } };
                    var log4 = new Log { DocumentId = "logs/1341-A", UserId = userId, Activities = new List<LogActivity> { } };
                    var log5 = new Log { DocumentId = "logs/1341-A", UserId = userId, Activities = null };
                    session.Store(log1);
                    session.Store(log2);
                    session.Store(log3);
                    session.SaveChanges();
                }
                WaitForIndexing(store); //If we want to query documents sometime we need to wait for the indexes to catch up
                //WaitForUserToContinueTheTest(store);//Sometimes we want to debug the test itself, this redirect us to the studio
                using (var session = store.OpenSession())
                {
                    var query = session.Query<Log>().Where(x => x.DocumentId == "logs/1231-A").ToList();
                    Assert.NotZero(query.Count);
                }

                List<string> suggestions = actSugService.GetSuggestions(userId, 20);
                Assert.AreEqual(activity1.Name, suggestions[0]);
                Assert.AreEqual(activity2.Name, suggestions[1]);
            }
        }
    }

    public class LogByName : AbstractIndexCreationTask<Log>
    {
        public LogByName()
        {
            Map = logs => from log in logs select new { log.DocumentId};
            Indexes.Add(x => x.DocumentId, FieldIndexing.Search);
        }
    }
}
