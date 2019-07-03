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
}