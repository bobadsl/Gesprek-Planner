using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace GesprekPlanner_WebApi.Hubs
{
    public class Test : Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendServerMessage("Send", $"{Context.ConnectionId}: {message}");
        }

        public async Task RedirectTo(string date)
        {
            await Clients.All.SendServerMessage(date);
        }

        public override async Task OnConnected()
        {
            await Clients.All.SendServerMessage($"{Context.ConnectionId} joined");
        }
    }
}
