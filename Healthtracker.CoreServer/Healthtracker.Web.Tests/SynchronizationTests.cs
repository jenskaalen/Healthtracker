using FakeItEasy;
using Healthtracker.Web.Model;
using Healthtracker.Web.Services.Synchronization;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Healthtracker.Web.Tests
{
    public class SynchronizationTests
    {
        [Test]
        public void SyncQueue_Pop_Adds_Pop_Removes()
        {
            var syncQueue = new SyncQueue();

            Assert.IsNull(syncQueue.Pop());

            ISynchronizationJob syncJob = A.Fake<ISynchronizationJob>();
            ISynchronizationJob syncJob2 = A.Fake<ISynchronizationJob>();
            syncQueue.Add(syncJob);
            syncQueue.Add(syncJob2);

            Assert.AreEqual(syncJob, syncQueue.Pop());
            //pop one more time
            syncQueue.Pop();

            Assert.IsNull(syncQueue.Pop());
        }
    }
}
