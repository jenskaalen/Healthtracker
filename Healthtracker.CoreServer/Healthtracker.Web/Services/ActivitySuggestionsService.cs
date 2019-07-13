using Healthtracker.Web.Model;
using Healthtracker.Web.Repositories;
using Raven.Client.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Healthtracker.Web.Services
{
    public class ActivitySuggestionsService : IActivitySuggestionsService
    {
        private readonly ILogRepository logRepository;
        private readonly IDocumentStore _store;

        public ActivitySuggestionsService(IDocumentStore store)
        {
            _store = store;
        }

        public List<string> GetSuggestions(string userId, int count)
        {
            using (var session = _store.OpenSession())
            {
                IEnumerable<LogActivity> result = session.Query<Log>().Where(x => x.UserId == userId && x.Activities != null && x.Activities.Count > 0).ToList().SelectMany(x => x.Activities)
                    .GroupBy(x => x.Name)
                  .OrderByDescending(g => g.Count())
                  .Take(count)
                  .SelectMany(g => g).ToList();
                
                return result.Select(x => x.Name).Distinct().ToList();
            }
        }
    }
}