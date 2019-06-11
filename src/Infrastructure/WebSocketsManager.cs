using ApplicationCore.Interfaces.Infastracture;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class WebSocketsManager
    {
        readonly NotificationsWebSocketsManager _notificationsWebSocketsManager;

        public WebSocketsManager(NotificationsWebSocketsManager notificationsWebSocketsManager)
        {
            _notificationsWebSocketsManager = notificationsWebSocketsManager;
        }

        public async Task HandleConnection(HttpContext httpContext)
        {
            if (httpContext.Request.Path.Equals("/notifications"))
            {
                await HandleNotificationsWebSocket(httpContext);
            }
            //if (httpContext.Request.Path.Equals("/otherWSSPath"))
            //    await HandleOtherWSSPath(httpContext);
            else
                httpContext.Response.StatusCode = 400;
        }

        private async Task HandleNotificationsWebSocket(HttpContext httpContext) =>
            await _notificationsWebSocketsManager.HandleConnection(httpContext);             
    }
}
