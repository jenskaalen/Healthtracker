using Healthtracker.Web.Model;
using Raven.Client.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Healthtracker.Web.Services
{
    public class SupplementSuggestionsService : ISupplementSuggestionsService
    {
        private readonly IDocumentStore _store;

        public SupplementSuggestionsService(IDocumentStore store)
        {
            _store = store;
        }
        
        public List<string> GetSuggestions(string userId, int count)
        {
            using (var session = _store.OpenSession())
            {
                IEnumerable<string> result = session.Query<Log>().Where(x => x.UserId == userId && x.Supplements != null && x.Supplements.Count > 0).ToList().SelectMany(x => x.Supplements)
                    .GroupBy(x => x)
                  .OrderByDescending(g => g.Count())
                  .Take(count)
                  .SelectMany(g => g).ToList();

                return result.Select(x => x).Distinct().ToList();
            }
        }
    }
}
