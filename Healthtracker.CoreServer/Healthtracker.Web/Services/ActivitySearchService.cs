using Healthtracker.Web.Model;
using Healthtracker.Web.Repositories;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Healthtracker.Web.Services
{
    public class ActivitySearchService : IActivitySearchService
    {
        private readonly IDocumentStore _store;

        public ActivitySearchService(IDocumentStore store)
        {
            _store = store;
        }

        public List<string> Search(string query, string userId, int count)
        {
            using (var session = _store.OpenSession())
            {
                List<Log> result = session.Query<Log>()
                    .Where(x => x.UserId == userId)
                    .Search(x => x.Activities, $"*{query}*")
                    .ToList();

                var activities = new List<LogActivity>();

                result.ForEach(x => {
                    var logs = x.Activities.Where(act => act.Name.Contains(query, StringComparison.InvariantCultureIgnoreCase));
                    logs.ToList().ForEach(log => activities.Add(log));
                    });

                return activities.Select(x => x.Name).Distinct(StringComparer.InvariantCultureIgnoreCase).Take(count).ToList();
            }
        }
    }
}
