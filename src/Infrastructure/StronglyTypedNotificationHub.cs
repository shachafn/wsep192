using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.Infastracture;
using ApplicationCore.Interfaces.ServiceLayer;
using Microsoft.AspNetCore.Http.Connections.Features;
using Microsoft.AspNetCore.Http.Connections.Internal;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Infrastructure
{
    public class StronglyTypedNotificationHub : Hub<INotificationClient>, IUserNotifier
    {
        private static Dictionary<Guid, Queue<string>> _userGuidToNotificationsQueue = new Dictionary<Guid, Queue<string>>();
        readonly ILogger<StronglyTypedNotificationHub> _logger;
        readonly IConnectionManager _connectionManager;

        public StronglyTypedNotificationHub(ILogger<StronglyTypedNotificationHub> logger,
            IConnectionManager connectionManager)
        {
            _logger = logger;
            _connectionManager = connectionManager;
        }

        public async Task SendMessageAsync(ICollection<Guid> targets, string message)
        {
            foreach (var target in targets)
            {
                await SendMessage(target, message);
            };
        }

        public async Task SendMessage(Guid target, string message)
        {
            var connectionid = _connectionManager.GetConnectionByUserGuid(target);
            if(connectionid == null)
            {
                Queue<string> notificationsQueue = GetNotificationsQueue(target);
                notificationsQueue.Enqueue(message);
                return;
            }
            await Clients.Client(connectionid).RecieveNotification(message);
        }

        public Task SendMessageToShopOwners(Guid shopGuid, string message)
        {
            return Clients.Group(shopGuid.ToString()).RecieveNotification(message);
        }

        public override async Task OnConnectedAsync()
        {
            var sessionId = GetSessionID(Context);
            _connectionManager.AddConnection(sessionId, Context.ConnectionId);
            await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");
            var userGuid = _connectionManager.GetUserGuidByConnectionId(Context.ConnectionId);
            Queue<string> notifications = GetNotificationsQueue(userGuid);
            foreach (var notification in notifications)
                await SendMessage(userGuid, notification);
            notifications.Clear();
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var sessionId = GetSessionID(Context);
            _connectionManager.RemoveConnection(sessionId);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnDisconnectedAsync(exception);
        }

        private Guid GetSessionID(HubCallerContext context)
        {
            var httpContext = ((HttpConnectionContext)Context.Features[typeof(IHttpContextFeature)]).HttpContext;
            return new Guid(httpContext.Session.Id);
        }

        private Queue<string> GetNotificationsQueue(Guid userGuid)
        {
            if (!_userGuidToNotificationsQueue.ContainsKey(userGuid))
                _userGuidToNotificationsQueue.Add(userGuid, new Queue<string>());
            return _userGuidToNotificationsQueue[userGuid];
        }
    }
}
