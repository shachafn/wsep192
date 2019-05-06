using System;
using System.Collections.Generic;

namespace ApplicationCore.Interfaces.ServiceLayer
{
    public interface IConnectionManager
    {
        string GetConnectionByUserGuid(Guid sessionId);
        void AddConnection(Guid sessionId, string connectionId);
        void RemoveConnection(Guid sessionId);
        void Clear();
        IEnumerator<KeyValuePair<Guid, string>> GetEnumerator();
        bool IsConnected(Guid userGuid);
        Guid GetSessionIdByConnectionId(string connectionId);
        Guid GetUserGuidByConnectionId(string connectionId);

    }
}