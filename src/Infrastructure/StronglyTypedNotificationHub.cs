using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.ServiceLayer;
using Microsoft.AspNetCore.Http.Connections.Features;
using Microsoft.AspNetCore.Http.Connections.Internal;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Infrastructure
{
    public class StronglyTypedNotificationHub : Hub<INotificationClient>
    {
        readonly ILogger<StronglyTypedNotificationHub> _logger;
        readonly NotificationsCenter _notificationsCenter;

        public StronglyTypedNotificationHub(ILogger<StronglyTypedNotificationHub> logger, NotificationsCenter notificationsCenter)
        {
            _logger = logger;
            _notificationsCenter = notificationsCenter;
        }

        public async Task SendMessageAsync(string connectionId, string message)
        {
            await Clients.Client(connectionId).RecieveNotification(message);
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            var notificationsQueue = _notificationsCenter.CheckForNotifications(Context.GetHttpContext());
            try
            {
                lock (notificationsQueue)
                {
                    while (notificationsQueue.Count != 0)
                    {
                        notificationsQueue.TryPeek(out var notification);
                        Clients.Client(Context.ConnectionId).RecieveNotification(notification);
                        notificationsQueue.TryDequeue(out string doesntMatter);
                    }
                }
            }
            catch { _logger.LogDebug($"Couldn't send notification."); }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
