using NUnit.Framework;
using Raven.TestDriver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Healthtracker.Web.Tests
{
    [SetUpFixture]
    public class TestSetup : RavenTestDriver
    {
        [OneTimeSetUp]  // [OneTimeSetUp] for NUnit 3.0 and up; see http://bartwullems.blogspot.com/2015/12/upgrading-to-nunit-30-onetimesetup.html
        public void SetUp()
        {
            ConfigureServer(new TestServerOptions
            {
                DataDirectory = "C:\\temp",
                FrameworkVersion = "2.2.6"
            });
        }
    }
}
