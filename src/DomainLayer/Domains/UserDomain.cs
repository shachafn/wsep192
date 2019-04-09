using DomainLayer.Data;
using DomainLayer.Data.Collections;
using DomainLayer.Data.Entitites;
using System;
using System.Collections.Generic;
using System.Linq;
using DomainLayer.Data.Entitites.Users.States;
using DomainLayer.Exceptions;
using DomainLayer.Data.Entitites.Users;

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
            DomainData.AllUsersCollection.Add(newUser.Guid, newUser);
            return true;
        }

        private bool IsUsernameTaken(string username) => DomainData.AllUsersCollection.Any(bUser => bUser.Username.Equals(username));


        /// <summary>
        /// Loggs the user in. Changes its state to Buyer (default).
        /// </summary>
        /// <param name="userGuid">Expected to be the const GuestGuid.</param>
        /// <returns></returns>
        public Guid Login(Guid userGuid, string username, string password)
        {
            if (!IsUserRegistered(username, password))
                return Guid.Empty;

            var user = new User(username, password);
            user.SetState(new BuyerUserState(username, password));
            LoggedInUsers.Add(user.Guid, user);
            return user.Guid;
        }

        private bool IsUserRegistered(string username, string password) => 
            DomainData.AllUsersCollection.Any(bUser => bUser.Username.Equals(username.ToLower()) && bUser.CheckPass(password));

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

    }
}
