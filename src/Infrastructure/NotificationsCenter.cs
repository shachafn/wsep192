using ApplicationCore.Data;
using ApplicationCore.Events;
using ApplicationCore.Interfaces.DomainLayer;
using ApplicationCore.Interfaces.ServiceLayer;
using DomainLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Utils;

namespace Infrastructure
{
    public class NotificationsCenter
    {
        readonly ILogger<NotificationsCenter> _logger;
        readonly ISessionManager _sessionManager;

        private static Dictionary<Guid, ConcurrentQueue<string>> _userGuidToNotificationsQueue = new Dictionary<Guid, ConcurrentQueue<string>>();

        public NotificationsCenter(ILogger<NotificationsCenter> logger, ISessionManager sessionManager)
        {
            _logger = logger;
            _sessionManager = sessionManager;
        }

        public ConcurrentQueue<string> CheckForNotifications(HttpContext httpContext)
        {
            var sessionId = httpContext.GetSessionID();
            var userGuid = _sessionManager.GetUserGuid(sessionId);
            if (userGuid == null) return new ConcurrentQueue<string>();
            if (!_userGuidToNotificationsQueue.ContainsKey(userGuid)) return new ConcurrentQueue<string>();
            return _userGuidToNotificationsQueue[userGuid];
        }

        public void HandleUpdate(IUpdateEvent updateEvent)
        {
            var targets = updateEvent.GetTargets(DomainData.ShopsCollection.Values, DomainData.RegisteredUsersCollection.Values);
            var msg = updateEvent.GetMessage();
            foreach (var target in targets)
            {
                if (!_userGuidToNotificationsQueue.ContainsKey(target))
                    _userGuidToNotificationsQueue.Add(target, new ConcurrentQueue<string>());
                _userGuidToNotificationsQueue[target].Enqueue(msg);
            }
        }
    }
}
