using ApplicationCore.Entities;
using ApplicationCore.Interfaces.ServiceLayer;
using Microsoft.AspNetCore.Http.Connections.Features;
using Microsoft.AspNetCore.Http.Connections.Internal;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using ServiceLayer.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure
{
    /// <summary>
    /// Singleton class responsible for managing connections of users.
    /// </summary>
    public class ConnectionManager : IEnumerable<KeyValuePair<Guid, string>>, IConnectionManager //IUserIdProvider
    {

        private static Dictionary<Guid, string> _sessionToConnectionDictionary = new Dictionary<Guid, string>();
        private ISessionManager _sessionManager;
        ILogger<ConnectionManager> _logger;

        public ConnectionManager(ILogger<ConnectionManager> logger, ISessionManager sessionManager)
        {
            _logger = logger;
            _sessionManager = sessionManager;
        }


        public string GetConnectionByUserGuid(Guid userGuid)
        {
            var sessionId = _sessionManager.GetSessionId(userGuid);
            if (_sessionToConnectionDictionary.ContainsKey(sessionId))
                return _sessionToConnectionDictionary[sessionId];
            else
                return null;
        }

        public Guid GetSessionIdByConnectionId(string connectionId)
        {
            if (_sessionToConnectionDictionary.ContainsValue(connectionId))
            {
                var userGuid = _sessionToConnectionDictionary.FirstOrDefault(guid => guid.Value.Equals(connectionId)).Key;
                return _sessionManager.GetSessionId(userGuid);
            }
            else
                return Guid.Empty;
        }

        public Guid GetUserGuidByConnectionId(string connectionId)
        {
            var sessionId = GetSessionIdByConnectionId(connectionId);
            return _sessionManager.GetUserGuid(sessionId);
        }

        public void AddConnection(Guid sessionId, string connectionId)
        {
            if (_sessionToConnectionDictionary.ContainsKey(sessionId))
                throw new InvalidOperationException($"A connection is already registered for session {sessionId}");

            _sessionToConnectionDictionary[sessionId] = connectionId;
        }

        public void RemoveConnection(Guid sessionId)
        {
            if (!_sessionToConnectionDictionary.ContainsKey(sessionId))
                throw new InvalidOperationException($"No connection registered for session {sessionId}");

            _sessionToConnectionDictionary.Remove(sessionId); 
        }

        public void Clear()
        {
            _sessionToConnectionDictionary.Clear();
        }

        public IEnumerator<KeyValuePair<Guid, string>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<Guid, string>>)_sessionToConnectionDictionary).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<Guid, string>>)_sessionToConnectionDictionary).GetEnumerator();
        }

        private Guid GetSessionID(HubConnectionContext context)
        {
            var httpContext = ((HttpConnectionContext)context.Features[typeof(IHttpContextFeature)]).HttpContext;
            return new Guid(httpContext.Session.Id);
        }

        bool IConnectionManager.IsConnected(Guid userGuid)
        {
            return _sessionToConnectionDictionary.ContainsKey(userGuid);
        }
    }
}
