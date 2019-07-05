using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Healthtracker.Web.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        private List<string> names = new List<string>();

        public async Task SendMessage(string user, string message)
        {
            string identifier = Context.UserIdentifier;
            
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public override Task OnConnectedAsync()
        {
            string name = Context.User.Identity.Name;
            names.Add(name);

            return base.OnConnectedAsync();
        }
    }
}
