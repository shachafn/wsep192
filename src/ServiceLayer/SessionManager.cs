using ApplicationCore.Entities;
using ServiceLayer.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceLayer
{
    /// <summary>
    /// Singleton class responsible for handling cookies (defining a Session).
    /// </summary>
    public class SessionManager
    {
        public Guid GuestGuid = new Guid("695D0341-3E62-4046-B337-2486443F311B");

        #region Singleton Implementation
        private static SessionManager instance = null;
        private static readonly object padlock = new object();
        public static SessionManager Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new SessionManager();
                    }
                    return instance;
                }
            }
        }
        #endregion
        public Dictionary<Guid, Guid> SessionToUserDictionary = new Dictionary<Guid, Guid>();
        public ICollection<Guid> SessionToGuestDictionary = new List<Guid>();
        public UserIdentifier ResolveCookie(Guid cookie)
        {
            if (SessionToUserDictionary.ContainsKey(cookie))
                return new UserIdentifier(SessionToUserDictionary[cookie], false);
            if (SessionToGuestDictionary.Contains(cookie))
                return new UserIdentifier(cookie, true);
            SessionToGuestDictionary.Add(cookie);
            return new UserIdentifier(cookie, true);
        }
        public Guid GetNewCookie() => Guid.NewGuid();

        public void SetLoggedIn(Guid cookie, Guid newUserGuid)
        {
            if (!SessionToGuestDictionary.Contains(cookie))
                throw new CookieNotFoundException($"Login - No Session with cookie {cookie} exists in the dictionary.");

            SessionToUserDictionary[cookie] = newUserGuid;
        }

        public void SetSessionLoggedOut(Guid cookie)
        {
            if (!SessionToUserDictionary.ContainsKey(cookie))
                throw new CookieNotFoundException($"Logout - No Session with cookie {cookie} exists in the dictionary.");

            SessionToUserDictionary.Remove(cookie); //remove the user from the logged in list
        }

        internal void SetUserLoggedOut(Guid userToRemoveGuid)
        {
            var result = SessionToUserDictionary.FirstOrDefault(s => s.Value.Equals(userToRemoveGuid));
            if (result.Equals(default(KeyValuePair<Guid, Guid>)))
                SessionToUserDictionary.Remove(result.Key);
        }

        public void Clear()
        {
            SessionToGuestDictionary.Clear();
            SessionToUserDictionary.Clear();
        }
    }
}
