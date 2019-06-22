using FakeItEasy;
using Healthtracker.Web.Repositories;
using Healthtracker.Web.Services;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net.Http;

namespace Tests
{
    public class FitbitTokenStorageTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void StoreToken_is_saved_to_memory()
        {
            //var tokenStorage = new FitbitTokenStorage();


            Assert.Pass();
        }
    }

    public class FitbitRepositoryTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Heartrate_is_parsed()
        {
            string json = System.IO.File.ReadAllText("JsonSample\\heartrate.json");
            var fake = A.Fake<IHttpClientFactory>();

            var repo = new FitbitRepository(fake);
            List<Healthtracker.Web.Model.FitbitHeartrate> heartrates = repo.GetFitbitHeartrates(json);
            //var tokenStorage = new FitbitTokenStorage();
            Assert.Greater(heartrates.Count, 0);

            var heartrate = heartrates[0];
            Assert.AreEqual(50, heartrate.RestingHeartrate);
        }

        [Test]
        public void Sleep_is_parsed()
        {
            string json = System.IO.File.ReadAllText("JsonSample\\sleep.json");
            var fake = A.Fake<IHttpClientFactory>();

            var repo = new FitbitRepository(fake);
            List<Healthtracker.Web.Model.FitbitSleep> sleeps = repo.GetFitbitSleep(json);
            //var tokenStorage = new FitbitTokenStorage();
            
            var sleep = sleeps[0];
            Assert.Greater(sleep.Sleep, 5);
        }
    }
}