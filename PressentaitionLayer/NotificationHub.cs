using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace PressentaitionLayer
{

    public class NotificationHub : Hub
    {
        #region NoticiationHubMethods
        public Task SendNotification(Guid userGuid, string notification)
        {
            return Clients.All.SendAsync("RecieveNotification", userGuid, notification);
        }

        public Task SendNoticiationToShopOwners(Guid shopGuid, string message)
        {
            return Clients.Group(shopGuid.ToString()).SendAsync("RecieveNotification", shopGuid ,message);
        }
        #endregion

        #region HubMethodName
        [HubMethodName("SendNotificationToUser")]
        public Task DirectMessage(Guid userGuid, string message)
        {
            return Clients.User(userGuid.ToString()).SendAsync("RecieveNotification", message);
        }
        #endregion

        #region ThrowHubException
        public Task ThrowException()
        {
            throw new HubException("This error will be sent to the client!");
        }
        #endregion

        #region OnConnectedAsync
        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "Shop Owners");
            await base.OnConnectedAsync();
        }
        #endregion

        #region OnDisconnectedAsync
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Shop Owners");
            await base.OnDisconnectedAsync(exception);
        }
        #endregion
    }
}
