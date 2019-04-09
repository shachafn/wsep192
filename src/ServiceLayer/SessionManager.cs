using ServiceLayer.Exceptions;
using System;
using System.Collections.Generic;
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
        public Guid ResolveCookie(Guid cookie) => SessionToUserDictionary.ContainsKey(cookie) ? SessionToUserDictionary[cookie] : GuestGuid;
        public Guid GetNewCookie() => Guid.NewGuid();

        public void SetLoggedIn(Guid cookie, Guid newUserGuid)
        {
            if (!SessionToUserDictionary.ContainsKey(cookie))
                throw new CookieNotFoundException($"No Session with cookie {cookie} exists in the dictionary.");

            SessionToUserDictionary[cookie] = newUserGuid;
        }

        public void SetLoggedOut(Guid cookie)
        {
            if (!SessionToUserDictionary.ContainsKey(cookie))
                throw new CookieNotFoundException($"No Session with cookie {cookie} exists in the dictionary.");

            SessionToUserDictionary[cookie] = GuestGuid;
        }
    }
}
