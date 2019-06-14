using ApplicationCore.Interfaces.ServiceLayer;
using Infrastructure;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace PresentaitionLayer
{
    public class PipelineManager
    {
        readonly ISessionManager _sessionManager;
        readonly WebSocketsManager _webSocketsManager;

        public PipelineManager(ISessionManager sessionManager, WebSocketsManager webSocketsManager)
        {
            _sessionManager = sessionManager;
            _webSocketsManager = webSocketsManager;
        }

        public async Task HandleHttpRequest(HttpContext httpContext, Func<Task> next)
        {
            if (httpContext.WebSockets.IsWebSocketRequest)
                await _webSocketsManager.HandleConnection(httpContext);
            else
                await next();
        }
    }
}
