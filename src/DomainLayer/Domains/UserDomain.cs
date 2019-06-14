using System;
using System.Linq;
using ApplicationCore.Interfaces.DomainLayer;
using ApplicationCore.Entities;
using Microsoft.Extensions.Logging;
using ApplicationCore.Data.Collections;
using ApplicationCore.Data;
using DomainLayer.Users;
using ApplicationCore.Entities.Users;
using System.Collections.Generic;
using ApplicationCore.Interfaces.DataAccessLayer;

namespace DomainLayer.Domains
{
    /// <summary>
    /// Singleton class to handle User logic.
    /// </summary>
    public class UserDomain : IUserDomain
    {
        private static LoggedInUsersEntityCollection LoggedInUsers = DomainData.LoggedInUsersEntityCollection;

        ILogger<UserDomain> _logger;
        IUnitOfWork _unirOfWork;
        ShopDomain _shopDomain;

        public UserDomain(ILogger<UserDomain> logger, IUnitOfWork unitOfWork, ShopDomain shopDomain)
        {
            _logger = logger;
            _unirOfWork = unitOfWork;
            _shopDomain = shopDomain;
        }

        public Guid Register(string username, string password, bool isAdmin)
        {
            if (IsUsernameTaken(username))
                return Guid.Empty;

            //var newUser = new BaseUser(username.ToLower(), password, isAdmin);
            var newUser = new BaseUser(username.ToLower(), password, isAdmin);
            _unirOfWork.BaseUserRepository.Add(newUser);
            return newUser.Guid;
        }

        public IUser GetUserObject(UserIdentifier userIdentifier)
        {
            if (userIdentifier.IsGuest)
            {
                var res = new GuestUser(userIdentifier.Guid, _unirOfWork, _shopDomain);
                DomainData.GuestsCollection.Add(res.Guid, res.Guid);
                return res;
            }
            var baseUser = _unirOfWork.BaseUserRepository.FindByIdOrNull(userIdentifier.Guid);
            if (baseUser.IsAdmin)
                return new AdminUser(baseUser, _unirOfWork, _shopDomain);
            return new RegisteredUser(baseUser, _unirOfWork, _shopDomain);
        }

        private bool IsUsernameTaken(string username)
        {
            var lower = username.ToLower();
            return _unirOfWork.BaseUserRepository.Query().Any(bUser => bUser.Username.Equals(lower));
        }


        public Guid Login(string username, string password)
        {
            BaseUser baseUser = GetRegisteredUserByUsernameOrNull(username);
            LoggedInUsers.Add(baseUser.Guid, baseUser.Guid);
            return baseUser.Guid;
        }

        private bool IsAdminCredentials(string username, string password)
        {
            var admin = _unirOfWork.BaseUserRepository.Query().First(bU => bU.IsAdmin);
            if (admin.Username.Equals(username.ToLower()) && admin.CheckPass(password))
                return true;
            return false;
        }

        private BaseUser GetRegisteredUserByUsernameOrNull(string username)
        {
            var lowerUsername = username.ToLower();
            return _unirOfWork.BaseUserRepository.Query().FirstOrDefault(r => string.Equals(r.Username, lowerUsername));
        }

        public bool LogoutUser(UserIdentifier userIdentifier)
        {
            LoggedInUsers.Remove(userIdentifier.Guid);
            return true;
        }

        public bool ChangeUserState(Guid userGuid, string newStateString)
        {
            return true;
        }

        public bool IsAdminExists() => _unirOfWork.BaseUserRepository.Query().Any(u => u.IsAdmin);

        public ICollection<BaseUser> GetAllUsersExceptMe(UserIdentifier userIdentifier)
        {
            return _unirOfWork.BaseUserRepository
                .Query()
                .Where(reg => !reg.Guid.Equals(userIdentifier.Guid))
                .ToList();
        }

        public IAdminUser GetAdminUser(UserIdentifier userIdentifier)
        {
            var baseUser = _unirOfWork.BaseUserRepository.FindByIdOrNull(userIdentifier.Guid);
            return new AdminUser(baseUser, _unirOfWork, _shopDomain);
        }
    }
}
