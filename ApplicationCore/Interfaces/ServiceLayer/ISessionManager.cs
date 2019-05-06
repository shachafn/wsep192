using System;
using ApplicationCore.Entities;

namespace ApplicationCore.Interfaces.ServiceLayer
{
    public interface ISessionManager
    {
        void Clear();
        UserIdentifier ResolveCookie(Guid cookie);
        void SetLoggedIn(Guid cookie, Guid newUserGuid);
        void SetSessionLoggedOut(Guid cookie);
        void SetUserLoggedOut(Guid userToRemoveGuid);
        Guid GetSessionId(Guid userGuid);
        Guid GetUserGuid(Guid sessionId);
    }
}