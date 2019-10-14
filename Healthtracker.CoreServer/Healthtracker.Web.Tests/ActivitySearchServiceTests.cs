using Healthtracker.Web.Model;
using Healthtracker.Web.Services;
using NUnit.Framework;
using Raven.Client.Documents;
using Raven.TestDriver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Healthtracker.Web.Tests
{
    public class ActivitySearchServiceTests: RavenTestDriver
    {
        const string UserId = "testUser";

        protected override void PreInitialize(IDocumentStore documentStore)
        {
            documentStore.Conventions.MaxNumberOfRequestsPerSession = 50;
            documentStore.Conventions.FindIdentityProperty = prop => prop.Name == "DocumentId";
        }

        [Test]
        public void Search_returns_results()
        {
            ConfigureServer(new TestServerOptions
            {
                DataDirectory = "C:\\temp",
                FrameworkVersion = "2.2.6"
            });

            using (var store = GetDocumentStore())
            {
                AddTestData(store);

                var searchservice = new ActivitySearchService(store);

                List<string> results = searchservice.Search("bo", UserId, 20);
                Assert.AreEqual(1, results.Count);
                Assert.AreEqual("boxing", results[0]);

                results = searchservice.Search("fi", UserId, 20);
                Assert.AreEqual(2, results.Count);
            }
        }

        private void AddTestData(IDocumentStore store)
        {
            
            var activity1 = new LogActivity()
            {
                Name = "boxing"
            };

            var activity2 = new LogActivity()
            {
                Name = "fishing"
            };

            var activity3 = new LogActivity()
            {
                Name = "fiddling"
            };

            //store.ExecuteIndex(new TestDocumentByName());
            using (var session = store.OpenSession())
            {
                var log1 = new Log { DocumentId = "logs/1341-A", UserId = UserId, Activities = new List<LogActivity> { activity1 } };
                var log2 = new Log { DocumentId = "logs/1231-A", UserId = UserId, Activities = new List<LogActivity> { activity2 } };
                var log3 = new Log { DocumentId = "logs/1255-A", UserId = UserId, Activities = new List<LogActivity> { activity1 } };
                var log6 = new Log { DocumentId = "logs/1256-A", UserId = UserId, Activities = new List<LogActivity> { activity3 } };
                //var log4 = new Log { DocumentId = "logs/1341-A", UserId = UserId, Activities = new List<LogActivity> { } };
                //var log5 = new Log { DocumentId = "logs/1341-A", UserId = UserId, Activities = null };
                session.Store(log1);
                session.Store(log2);
                session.Store(log3);
                session.Store(log6);
                session.SaveChanges();
            }
            WaitForIndexing(store); //If we want to query documents sometime we need to wait for the indexes to catch up
        }
    }
}
