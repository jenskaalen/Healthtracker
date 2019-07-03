using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Healthtracker.Web.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            string identifier = Context.UserIdentifier;
            
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
