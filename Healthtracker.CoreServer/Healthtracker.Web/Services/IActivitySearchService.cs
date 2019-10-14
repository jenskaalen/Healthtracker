using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Healthtracker.Web.Services
{
    public interface IActivitySearchService
    {
        List<string> Search(string query, string userId, int count);
    }
}
