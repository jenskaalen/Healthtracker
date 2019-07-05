using Healthtracker.Web.Model;

namespace Healthtracker.Web.Services.Synchronization
{
    public interface ISyncQueue
    {
        ISynchronizationJob Pop();

        void Add(ISynchronizationJob job);
    }
}