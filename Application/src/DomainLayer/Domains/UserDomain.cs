﻿using System;
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

namespace DomainLayer.Domains
{
    /// <summary>
    /// Singleton class to handle User logic.
    /// </summary>
    public class UserDomain : IUserDomain
    {
        private static LoggedInUsersEntityCollection LoggedInUsers = DomainData.LoggedInUsersEntityCollection;
        private static ShopsEntityCollection Shops = DomainData.ShopsCollection;


        ILogger<UserDomain> _logger;
        public UserDomain(ILogger<UserDomain> logger)
        {
            _logger = logger;
        }

        public Guid Register(string username, string password, bool isAdmin)
        {
            if (IsUsernameTaken(username))
                return Guid.Empty;

            //var newUser = new BaseUser(username.ToLower(), password, isAdmin);
            var newUser = new BaseUser(username, password, isAdmin);
            DomainData.RegisteredUsersCollection.Add(newUser.Guid, newUser);
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

        private bool IsUsernameTaken(string username) => DomainData.RegisteredUsersCollection.Any(bUser => bUser.Username.ToLower().Equals(username.ToLower()));


        public Guid Login(string username, string password)
        {
            if (IsAdminCredentials(username, password)) return LoginAdmin(username, password);

            BaseUser baseUser = GetRegisteredUserByUsername(username);
            var user = new RegisteredUser(baseUser);
            LoggedInUsers.Add(user.Guid, user);
            ChangeUserState(user.Guid, BuyerUserState.BuyerUserStateString);
            return user.Guid;
        }

        private bool IsAdminCredentials(string username, string password)
        {
            var admin = DomainData.RegisteredUsersCollection.First(bU => bU.IsAdmin);
            if (admin.Username.Equals(username.ToLower()) && admin.CheckPass(password))
                return true;
            return false;
        }

        private Guid LoginAdmin(string username, string password)
        {
            if (!DomainData.LoggedInUsersEntityCollection.Any(u => u.IsAdmin))
            {
                BaseUser baseUser = GetRegisteredUserByUsername(username);
                var user = new RegisteredUser(baseUser);
                LoggedInUsers.Add(user.Guid, user);
                return baseUser.Guid;
            }
            return DomainData.RegisteredUsersCollection.First(bU => bU.IsAdmin).Guid;
        }

        private BaseUser GetRegisteredUserByUsername(string username)
        {
            return DomainData.RegisteredUsersCollection.First(r => string.Equals(r.Username.ToLower(), username.ToLower()));
        }

        public bool LogoutUser(UserIdentifier userIdentifier)
        {
            LoggedInUsers.Remove(userIdentifier.Guid);
            return true;
        }

        public bool ChangeUserState(Guid userGuid, string newStateString)
        {
            var user = DomainData.LoggedInUsersEntityCollection[userGuid];
            var builder = new StateBuilder();
            var newState = builder.BuildState(newStateString, user);
            return user.SetState(newState);
        }

        public bool IsAdminExists() => DomainData.RegisteredUsersCollection.Any(u => u.IsAdmin);

        public ICollection<BaseUser> GetAllUsersExceptMe(UserIdentifier userIdentifier)
        {
            return DomainData.RegisteredUsersCollection
                .Where(reg => !reg.Guid.Equals(userIdentifier.Guid))
                .ToList();
        }
    }
}