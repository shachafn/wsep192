using DomainLayer.Data;
using DomainLayer.Data.Collections;
using DomainLayer.Data.Entitites;
using System;
using System.Collections.Generic;
using System.Linq;
using DomainLayer.Data.Entitites.Users.States;
using DomainLayer.Exceptions;
using DomainLayer.Data.Entitites.Users;
using System.Diagnostics;

namespace DomainLayer.Domains
{
    /// <summary>
    /// Singleton class to handle User logic.
    /// </summary>
    public class UserDomain
    {
        private static LoggedInUsersEntityCollection LoggedInUsers = DomainData.LoggedInUsersEntityCollection;
        private static ShopEntityCollection Shops = DomainData.ShopsCollection;


        #region Singleton Implementation
        private static UserDomain instance = null;
        private static readonly object padlock = new object();
        public static UserDomain Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new UserDomain();
                    }
                    return instance;
                }
            }
        }
        #endregion


        /// <summary>
        /// Registeres the user.
        /// </summary>
        /// <returns></returns>
        public bool Register(string username, string password)
        {
            if (IsUsernameTaken(username))
                return false;

            var newUser = new BaseUser(username.ToLower(), password);
            DomainData.RegisteredUsersCollection.Add(newUser.Guid, newUser);
            return true;
        }

        private bool IsUsernameTaken(string username) => DomainData.RegisteredUsersCollection.Any(bUser => bUser.Username.Equals(username));


        /// <summary>
        /// Loggs the user in. Changes its state to Buyer (default).
        /// </summary>
        /// <param name="userGuid">Expected to be the const GuestGuid.</param>
        /// <returns></returns>
        public Guid Login(Guid userGuid, string username, string password)
        {
            BaseUser baseUser = VerifyRegisteredUser(username, password);
            var user = new User(baseUser);
            LoggedInUsers.Add(user.Guid, user);
            return user.Guid;
        }

        /// <summary>
        /// Loggs the user out.
        /// </summary>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        public bool LogoutUser(Guid userGuid)
        {

            if (!LoggedInUsers.ContainsKey(userGuid))
                throw new UserNotFoundException($"Could not find a logged in user with guid {userGuid}");

            LoggedInUsers.Remove(userGuid);
            return true;
        }

        public bool ChangeUserState(Guid userGuid, string newStateString)
        {
            var user = DomainData.LoggedInUsersEntityCollection[userGuid];
            var builder = new StateBuilder();
            var newState = builder.BuildState(newStateString, user);
            return user.SetState(newState);
        }

        #region Verifiers
        private BaseUser VerifyRegisteredUser(string username, string password)
        {
            var user = DomainData.RegisteredUsersCollection.
                FirstOrDefault(bUser => bUser.Username.Equals(username.ToLower()) && bUser.CheckPass(password));
            if (user == null)
            {
                StackTrace stackTrace = new StackTrace();
                throw new Exception($"No registered user with username - {username.ToLower()}, password = {password}" +
        $"Cant complete {stackTrace.GetFrame(1).GetMethod().Name}");
            }
            return user;
        }

        #endregion
    }
}
