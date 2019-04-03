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

        public Guid GuestGuid = new Guid("695D0341-3E62-4046-B337-2486443F311B");

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
            if (username == null || password == null || username.Equals(string.Empty) || password.Equals(string.Empty))
                return false;

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
            if (!userGuid.Equals(GuestGuid))
                return Guid.Empty;

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
            if (userGuid.Equals(GuestGuid)) //Cant logout a guest
                return false;

            if (!LoggedInUsers.ContainsKey(userGuid))
                throw new UserNotFoundException($"Could not find a logged in user with guid {userGuid}");

            LoggedInUsers.Remove(userGuid);
            return true;
        }

        public Guid OpenShopForUser(Guid userGuid)
        {
            if (!LoggedInUsers.ContainsKey(userGuid))
                throw new UserNotFoundException($"Could not find a logged in user with guid {userGuid}");

            return LoggedInUsers[userGuid].OpenShop();
        }

        public bool RemoveUser(Guid userGuid, Guid userToRemoveGuid)
        {
            if (!LoggedInUsers.ContainsKey(userGuid))
                throw new UserNotFoundException($"Could not find a logged in user with guid {userGuid}");

            return LoggedInUsers[userGuid].RemoveUser(userToRemoveGuid);
        }
    }

        private bool UserIsTheOnlyOwnerOfAnActiveShop(Guid userGuid)
        {
            return Shops.Any(shop =>
            {
                var isOwner = shop.Owners.Any(owner => owner.Guid.Equals(userGuid));
                return isOwner && shop.Owners.Count > 1;
            });
        }

        public void RemoveShopOfUserByShopGuid(Guid shopGuid, string username)
        {
            var user = LoggedInUsers.GetUserByUsername(username);
            if (user == null) return;
            var shop = user.ShopsOwned.FirstOrDefault(lShop => lShop.Guid.Equals(shopGuid));
            if (shop != null)
                user.ShopsOwned.Remove(shop);
        }
        public void RemoveShopOfUserByShopGuid(Guid shopGuid, Guid userGuid)
        {
            var user = LoggedInUsers[userGuid];
            if (user == null) return;
            var shop = user.ShopsOwned.FirstOrDefault(lShop => lShop.Guid.Equals(shopGuid));
            if (shop != null)
                user.ShopsOwned.Remove(shop);
        }

        public int GetUsersCount()
        {
            return LoggedInUsers.Count;
        }

        public void AddToUsersPurchaseHistory(ShoppingBag shoppingBag, Guid userGuid)
        {
            var user = LoggedInUsers[userGuid];
            if (user == null) return;
            user.PurchaseHistory.Add(shoppingBag);
        }

        public bool PurchaseCart(Guid userGuid, Guid shopGuid)
        {
            throw new NotImplementedException();
        }

        public bool IsAdminUser(Guid guid) => LoggedInUsers.Any(user => user.Username.Equals(guid) && user.IsAdmin);
        public bool ExistsAdminUser() => LoggedInUsers.Any(user => user.IsAdmin);

        public bool AddProductToShop(Guid userGuid, string productName, string productCategory,
    double price, int quantity)
        {
            if (!IsUserExists(userGuid)) throw new UserNotFo


        }
    }
}
