using Healthtracker.Web.Hubs;
using Healthtracker.Web.Model;
using Healthtracker.Web.Services.Synchronization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Healthtracker.Web.Services
{
    public class SynchronizationService : BackgroundService
    {
        private List<ISynchronizationJob> _jobs = new List<ISynchronizationJob>();

        private readonly IHubContext<NotificationHub> hubContext;
        private readonly ISyncQueue syncQueue;

        public DateTime Created { get; }

        public SynchronizationService(IHubContext<NotificationHub> hubContext, ISyncQueue syncQueue)
        {
            this.hubContext = hubContext;
            this.syncQueue = syncQueue;
            Created = DateTime.Now;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                Model.ISynchronizationJob job = syncQueue.Pop();

                if (job != null)
                {
                    IClientProxy user = this.hubContext.Clients.User(job.UserId);
                    await user.SendAsync("ReceiveMessage", "beee", $"{job.Name} is starting...");
                    await job.Start();

                    await user.SendAsync("ReceiveMessage", "beee", $"{job.Name} synchronization has finished");
                }

                await Task.Delay(500);
            }
        }
    }
}
