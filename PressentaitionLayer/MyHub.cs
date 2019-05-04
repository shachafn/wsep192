using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PressentaitionLayer
{
    public class MyHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", "Hayounous", "U r all dead", "Error");
        }

        public override async Task OnConnectedAsync()
        {
            for (int i = 1; i < 10; i++)
            {
                await Clients.Caller.SendAsync("ReceiveMessage", "Welcome to AviExpress");
                Thread.Sleep(70*i);
            }
        }

    }
}