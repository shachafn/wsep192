﻿using DomainLayer.Data;
using DomainLayer.Data.Collections;
using DomainLayer.Data.Entitites;
using System;
using System.Collections.Generic;
using System.Linq;
using DomainLayer.Data.Entitites.Users.States;
using DomainLayer.Data.Entitites.Users;
using DomainLayer.Entitites.Users;
using ApplicationCore.Interfaces.DomainLayer;
using ApplicationCore.Entities;

namespace DomainLayer.Domains
{
    /// <summary>
    /// Singleton class to handle User logic.
    /// </summary>
    public class UserDomain : IUserDomain
    {
        private static LoggedInUsersEntityCollection LoggedInUsers = DomainData.LoggedInUsersEntityCollection;
        private static ShopsEntityCollection Shops = DomainData.ShopsCollection;


        public Guid Register(string username, string password, bool isAdmin)
        {
            if (IsUsernameTaken(username))
                return Guid.Empty;

            var newUser = new BaseUser(username.ToLower(), password, isAdmin);
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
            BaseUser baseUser = GetRegisteredUserByUsername(username);
            var user = new RegisteredUser(baseUser);
            LoggedInUsers.Add(user.Guid, user);
            ChangeUserState(user.Guid, BuyerUserState.BuyerUserStateString);
            return user.Guid;
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

    }
}
