using ApplicationCore.Entities;
using ApplicationCore.Interfaces.ServiceLayer;
using Microsoft.Extensions.Logging;
using ServiceLayer.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServiceLayer
{
    /// <summary>
    /// Singleton class responsible for handling cookies (defining a Session).
    /// </summary>
    public class SessionManager : ISessionManager
    {
        ILogger<SessionManager> _logger;
        public SessionManager(ILogger<SessionManager> logger)
        {
            _logger = logger;
        }
        public static Dictionary<Guid, Guid> SessionToUserDictionary = new Dictionary<Guid, Guid>();
        public static ICollection<Guid> SessionToGuestDictionary = new List<Guid>();
        public UserIdentifier ResolveCookie(Guid cookie)
        {
            if (SessionToUserDictionary.ContainsKey(cookie))
                return new UserIdentifier(SessionToUserDictionary[cookie], false);
            if (SessionToGuestDictionary.Contains(cookie))
                return new UserIdentifier(cookie, true);
            SessionToGuestDictionary.Add(cookie);
            return new UserIdentifier(cookie, true);
        }

        public void SetLoggedIn(Guid cookie, Guid newUserGuid)
        {
            if (!SessionToGuestDictionary.Contains(cookie))
                throw new CookieNotFoundException($"Login - No Session with cookie {cookie} exists in the dictionary.");

            SessionToUserDictionary[cookie] = newUserGuid;
            SessionToGuestDictionary.Remove(cookie);
        }

        public void SetSessionLoggedOut(Guid cookie)
        {
            if (!SessionToUserDictionary.ContainsKey(cookie))
                throw new CookieNotFoundException($"Logout - No Session with cookie {cookie} exists in the dictionary.");

            SessionToUserDictionary.Remove(cookie); //remove the user from the logged in list
        }

        public void SetUserLoggedOut(Guid userToRemoveGuid)
        {
            var result = SessionToUserDictionary.FirstOrDefault(s => s.Value.Equals(userToRemoveGuid));
            if (result.Equals(default(KeyValuePair<Guid, Guid>)))
                SessionToUserDictionary.Remove(result.Key);
        }

        public Guid GetSessionId(Guid userGuid)
        {
            var result = SessionToUserDictionary.FirstOrDefault(s => s.Value.Equals(userGuid));
            return result.Key;
        }

        public void Clear()
        {
            SessionToGuestDictionary.Clear();
            SessionToUserDictionary.Clear();
        }

        public Guid GetUserGuid(Guid sessionId)
        {
            SessionToUserDictionary.TryGetValue(sessionId, out var userGuid);
            return userGuid;
        }
    }
}
