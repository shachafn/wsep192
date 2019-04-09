using ServiceLayer.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceLayer
{
    /// <summary>
    /// Singleton class responsible for handling cookies (defining a Session).
    /// The class maps between the system-user's cookie to the actual user object if the system-user
    /// logged in, or to the GuestGuid if he hasn't logged in.
    /// </summary>
    public class SessionManager
    {
        private Dictionary<Guid, Guid> _sessionToUserDictionary = new Dictionary<Guid, Guid>();
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

        public Guid ResolveCookie(Guid cookie) => _sessionToUserDictionary.ContainsKey(cookie) ? _sessionToUserDictionary[cookie] : GuestGuid;
        public Guid GetNewCookie() => Guid.NewGuid();

        public void SetLoggedIn(Guid cookie, Guid newUserGuid)
        {
            if (!_sessionToUserDictionary.ContainsKey(cookie))
                throw new CookieNotFoundException($"No Session with cookie {cookie} exists in the dictionary.");

            _sessionToUserDictionary[cookie] = newUserGuid;
        }

        public void SetLoggedOut(Guid cookie)
        {
            if (!_sessionToUserDictionary.ContainsKey(cookie))
                throw new CookieNotFoundException($"No Session with cookie {cookie} exists in the dictionary.");

            _sessionToUserDictionary[cookie] = GuestGuid;
        }
    }
}
