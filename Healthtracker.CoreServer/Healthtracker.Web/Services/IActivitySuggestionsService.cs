using Healthtracker.Web.Model;
using Healthtracker.Web.Repositories;
using Raven.Client.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Healthtracker.Web.Services
{
    public interface IActivitySuggestionsService
    {
        List<string> GetSuggestions(string userId, int count);
    }
}
