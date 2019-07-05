using System.Threading.Tasks;

namespace Healthtracker.Web.Model
{
    public interface ISynchronizationJob
    {
        string UserId { get; }
        string Name { get; }

        Task Start();
    }
}