using ApplicationCore.Entities;
using ApplicationCore.Entities.Users;
using System;

namespace ApplicationCore.Interfaces.DomainLayer
{
    public interface IUserDomain
    {
        bool ChangeUserState(Guid userGuid, string newStateString);
        IUser GetUserObject(UserIdentifier userIdentifier);
        bool IsAdminExists();
        Guid Login(string username, string password);
        bool LogoutUser(UserIdentifier userIdentifier);
        Guid Register(string username, string password, bool isAdmin);
    }
}