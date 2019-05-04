using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Infrastructure
{
    public class StronglyTypedNotificationHub : Hub<INotificationClient>
    {
        public async Task SendMessage(Guid userGuid, string message)
        {
            await Clients.All.RecieveNotification(userGuid, message);
        }

        public Task SendMessageToShopOwners(Guid shopGuid, string message)
        {
            return Clients.Group(shopGuid.ToString()).RecieveNotification(message);
        }

        public override async Task OnConnectedAsync()
        {
            for (int i = 1; i < 10; i++)
            {
                await Clients.Caller.RecieveNotification("Welcome to AviExpress");
                Thread.Sleep(70 * i);
            }
            await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
