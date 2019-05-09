using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Data;
using ApplicationCore.Events;
using ApplicationCore.Interfaces.Infastracture;
using ApplicationCore.Interfaces.ServiceLayer;
using Microsoft.AspNetCore.Http;
using Utils;

namespace Infrastructure
{
    public class NotificationsWebSocketsManager : IUserNotifier
    {
        readonly static ConcurrentDictionary<Guid, WebSocket> _sessionIdToWebSocket = new ConcurrentDictionary<Guid, WebSocket>();
        readonly static Dictionary<Guid, ConcurrentQueue<string>> _userGuidToNotificationsQueue = new Dictionary<Guid, ConcurrentQueue<string>>();

        readonly ISessionManager _sessionManager;

        public NotificationsWebSocketsManager(ISessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }

        public async Task HandleConnection(HttpContext httpContext)
        {
            var sessionId = httpContext.GetSessionID();
            WebSocket webSocket = await httpContext.WebSockets.AcceptWebSocketAsync();
            _sessionIdToWebSocket.TryAdd(sessionId, webSocket);
            await NotifyUserOnPastNotificaTions(webSocket, sessionId);


            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            while (!result.CloseStatus.HasValue)
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
            _sessionIdToWebSocket.TryRemove(httpContext.GetSessionID(),out var idontcare);
        }

        private async Task NotifyUserOnPastNotificaTions(WebSocket webSocket, Guid sessionId)
        {
            var userGuid = _sessionManager.GetUserGuid(sessionId);
            if (userGuid != null)
            {
                if (_userGuidToNotificationsQueue.ContainsKey(userGuid))
                {
                    var queue = _userGuidToNotificationsQueue[userGuid];
                    while (queue.Count != 0)
                    {
                        queue.TryPeek(out var notification);
                        if (notification != null)
                        {
                            await SendMessage(webSocket, notification);
                            queue.TryDequeue(out var idc);
                        }
                    }
                }
            }
        }

        public async Task NotifyEvent(IUpdateEvent updateEvent)
        {
            var targets = updateEvent.Targets;
            var msg = updateEvent.Message;
            var initiatorGuid = updateEvent.Initiator;
            //For Initiator we dont want to send immediately because its page will be refreshed
            if (!_userGuidToNotificationsQueue.ContainsKey(initiatorGuid))
                _userGuidToNotificationsQueue.Add(initiatorGuid, new ConcurrentQueue<string>());
            _userGuidToNotificationsQueue[initiatorGuid].Enqueue(msg);
            if (targets.Contains(initiatorGuid)) targets.Remove(initiatorGuid);

            foreach (var target in targets)
            {
                var targetSessionid = _sessionManager.GetSessionId(target);
                if (targetSessionid != null)
                {
                    if (_sessionIdToWebSocket.ContainsKey(targetSessionid))
                    {
                        var socket = _sessionIdToWebSocket[targetSessionid];
                        await SendMessage(socket, msg);
                    }
                }
                else
                {
                    if (!_userGuidToNotificationsQueue.ContainsKey(target))
                        _userGuidToNotificationsQueue.Add(target, new ConcurrentQueue<string>());
                    _userGuidToNotificationsQueue[target].Enqueue(msg);
                }
            }
        }

        public async Task SendMessage(WebSocket webSocket, string msg)
        {
            var msgBytes = Encoding.UTF8.GetBytes(msg);
            var buffer = new ArraySegment<Byte>(msgBytes, 0, msgBytes.Length);
            await webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}
