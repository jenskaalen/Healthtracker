using Healthtracker.Web.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Healthtracker.Web.Services.Synchronization
{
    public class SyncQueue: ISyncQueue
    {
        private List<ISynchronizationJob> _jobs = new List<ISynchronizationJob>();

        public void Add(ISynchronizationJob job)
        {
            _jobs.Add(job);
        }

        public ISynchronizationJob Pop()
        {
            var job = _jobs.FirstOrDefault();

            if (job != null)
            {
                _jobs.Remove(job);
                return job;
            }
            else
                return null;
        }
    }
}
