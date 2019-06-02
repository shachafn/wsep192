using System;
using System.Linq;
using ApplicationCore.Interfaces.DomainLayer;
using ApplicationCore.Entities;
using Microsoft.Extensions.Logging;
using ApplicationCore.Data.Collections;
using ApplicationCore.Data;
using DomainLayer.Users;
using DomainLayer.Users.States;
using DomainLaye.Users.States;
using ApplicationCore.Entities.Users;
using DomainLayer.Data.Entitites.Users.States;
using System.Collections.Generic;
using ApplicationCore.Interfaces.DAL;

namespace DomainLayer.Domains
{
    /// <summary>
    /// Singleton class to handle User logic.
    /// </summary>
    public class UserDomain : IUserDomain
    {
        private static LoggedInUsersEntityCollection LoggedInUsers = DomainData.LoggedInUsersEntityCollection;
        private IUnitOfWork _unitOfWork;

        ILogger<UserDomain> _logger;
        public UserDomain(ILogger<UserDomain> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public Guid Register(string username, string password, bool isAdmin)
        {
            if (IsUsernameTaken(username))
                return Guid.Empty;

            //var newUser = new BaseUser(username.ToLower(), password, isAdmin);
            var newUser = new BaseUser(username, password, isAdmin);
            _unitOfWork.UserRepository.Create(newUser);
            return newUser.Guid;
        }

        public IUser GetUserObject(UserIdentifier userIdentifier)
        {
            if (userIdentifier.IsGuest)
            {
                if (DomainData.GuestsCollection.TryGetValue(userIdentifier.Guid, out IUser res))
                    return res;
                res = new GuestUser(userIdentifier.Guid);
                DomainData.GuestsCollection.Add(res.Guid,res);
                return res;
            }
            return DomainData.LoggedInUsersEntityCollection[userIdentifier.Guid];
        }

        private bool IsUsernameTaken(string username) => _unitOfWork.UserRepository.FindAll().Any(bUser => bUser.Username.ToLower().Equals(username.ToLower()));


        public Guid Login(string username, string password)
        {
            if (IsAdminCredentials(username, password)) return LoginAdmin(username, password);

            BaseUser baseUser = GetRegisteredUserByUsername(username);
            var user = new RegisteredUser(baseUser, _unitOfWork);
            LoggedInUsers.Add(user.Guid, user);
            ChangeUserState(user.Guid, BuyerUserState.BuyerUserStateString);
            return user.Guid;
        }

        private bool IsAdminCredentials(string username, string password)
        {
            var admin = _unitOfWork.UserRepository.FindAll().First(bU => bU.IsAdmin);
            if (admin.Username.Equals(username.ToLower()) && admin.CheckPass(password))
                return true;
            return false;
        }

        private Guid LoginAdmin(string username, string password)
        {
            if (!DomainData.LoggedInUsersEntityCollection.Any(u => u.IsAdmin))
            {
                BaseUser baseUser = GetRegisteredUserByUsername(username);
                var user = new RegisteredUser(baseUser, _unitOfWork);
                LoggedInUsers.Add(user.Guid, user);
                return baseUser.Guid;
            }
            return _unitOfWork.UserRepository.FindAll().First(bU => bU.IsAdmin).Guid;
        }

        private BaseUser GetRegisteredUserByUsername(string username)
        {
            return _unitOfWork.UserRepository.FindAll().First(r => string.Equals(r.Username.ToLower(), username.ToLower()));
        }

        public bool LogoutUser(UserIdentifier userIdentifier)
        {
            LoggedInUsers.Remove(userIdentifier.Guid);
            return true;
        }

        public bool ChangeUserState(Guid userGuid, string newStateString)
        {
            var user = DomainData.LoggedInUsersEntityCollection[userGuid];
            var builder = new StateBuilder(_unitOfWork);
            var newState = builder.BuildState(newStateString, user, _unitOfWork);
            return user.SetState(newState);
        }

        public bool IsAdminExists() => _unitOfWork.UserRepository.FindAll().Any(u => u.IsAdmin);

        public ICollection<BaseUser> GetAllUsersExceptMe(UserIdentifier userIdentifier)
        {
            return _unitOfWork.UserRepository
                .FindAll()
                .Where(reg => !reg.Guid.Equals(userIdentifier.Guid))
                .ToList();
        }
    }
}
