using Healthtracker.Web.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Healthtracker.Web.Repositories
{
    public interface ILogRepository
    {
        Log Create(Log log);
        void Delete(int id);
        Log Update(Log log);
        List<Log> GetAll(string userId);
        List<Log> Get(int logsPerPage, int logsToSkip, string userId);
    }
}
