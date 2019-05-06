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
        readonly ILogger<StronglyTypedNotificationHub> _logger;
        readonly IConnectionManager _connectionManager;

        public StronglyTypedNotificationHub(ILogger<StronglyTypedNotificationHub> logger,
            IConnectionManager connectionManager)
        {
            _logger = logger;
            _connectionManager = connectionManager;
        }

        public async Task SendMessage(ICollection<Guid> targets, string message)
        {
            foreach (var target in targets)
            {
                var connectionid = _connectionManager.GetConnectionByUserGuid(target);
                await Clients.Client(connectionid).RecieveNotification(message);
            };
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
    }
}
