using DomainLayer.Data;
using DomainLayer.Data.Collections;
using DomainLayer.Data.Entitites;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace DomainLayer.Domains
{
    public static class UserDomain
    {
        private static UserEntityCollection Users = DomainData.UsersCollection;


        /// <summary>
        /// logs the user in , changes its logged field to true , and retrieves all it's stored information
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>true if the login was sucseesful, false if one or more of the parameters were wrong 
        /// or if the user is already connected</returns>
        public static bool Login(string username, string password)
        {
            if (!ExistsUser(username,password))
                return false;

            var user = Users.GetUserByUsername(username);
            if (user.IsLoggedIn)
                return false;

            user.IsLoggedIn = true;
            return true;
        }

        public static bool ExistsUser(Guid guid) => Users[guid] != null;

        public static bool ExistsUser(string username, string password)
        {
            var user = Users.GetUserByUsername(username);
            if (user == null) return false;
            return user.CheckPass(password);
        }

        public static bool ExistsUser(string username)
        {
            var user = Users.GetUserByUsername(username);
            if (user == null) return false;
            return true;
        }

        /// <summary>
        /// This method logs the user out , and saves it's changed properties
        /// </summary>
        /// <returns>True if the user is logged-in, false otherwise.</returns>
        public static bool LogoutUser(string username)
        {
            if (!ExistsUser(username))
                return false;

            Users.GetUserByUsername(username).IsLoggedIn = false;
            return true;
        }

        /// <summary>
        ///  if the username is not taken already ,
        ///  the method creates a new user with this credentials and stores it in the users list
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password">a string for the password, should be at least 6 characters long</param>
        /// <returns> returns the created user or null otherwise</returns>
        public static User Register(string username, string password)
        {
            if (!ExistsUser(username) || password.Length < 6)
                return null;

            User newUser = new User(username, password, false);
            Users.Add(newUser.Guid, newUser);
            return newUser;
        }

        //Why we need that in this 'UserDomain' class?
        /*
        /// <summary>
        /// searches products according to a user recived string and filters them if a filter is given
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="filters"> an optional argument , the results of the search will be filtered using all of these</param>
        /// <returns>returns the list of products found (an empty list if found none)</returns>
        List<Product> Search(string searchString, List<ProductFilter> filters = null)
        {
            List<Product> productsFound = new List<Product>();
            foreach (Shop shop in _shopsOwned)
            {
                productsFound.AddRange(shop.SearchProducts(searchString));
            }
            foreach (ProductFilter filter in filters)
            {
                filter.ApplyFilter(productsFound);
            }
            return productsFound;
        }
        */

        public static Guid OpenShopForUser(Guid guid)
        {
            if (!ExistsUser(guid))
                return Guid.Empty;

            var user = Users[guid];
            if (user.IsLoggedIn)
                return Guid.Empty;

            var shop = new Shop();
            user.ShopsOwned.Add(shop);
            return shop.ShopGuid;
        }

        public static Guid OpenShopForUser(string username)
        {
            if (!ExistsUser(username))
                return Guid.Empty;

            var user = Users.GetUserByUsername(username);
            if (user.IsLoggedIn)
                return Guid.Empty;

            var shop = new Shop();
            user.ShopsOwned.Add(shop);
            return shop.ShopGuid;
        }

        public static bool RemoveUser(string username)
        {
            var user = Users.GetUserByUsername(username);
            if (user == null) return false;
            if (UserIsTheOnlyOwnerOfAnActiveShop(username))
                return false;
            //TODO: ACTUALLY REMOVE USER
            return false;
        }

        private static bool UserIsTheOnlyOwnerOfAnActiveShop(string username)
        {
            return Shop._shops.Any(shop =>
            {
                var isOwner = shop.Value.Owners.Any(owner => owner.Username.Equals(username));
                return isOwner && shop.Value.Owners.Count > 1;
            });
        }

        public static void RemoveShopOfUserByShopGuid(Guid shopGuid, string username)
        {
            var user = Users.GetUserByUsername(username);
            if (user == null) return;
            var shop = user.ShopsOwned.FirstOrDefault(lShop => lShop.ShopGuid.Equals(shopGuid));
            if (shop != null)
                user.ShopsOwned.Remove(shop);
        }
        public static void RemoveShopOfUserByShopGuid(Guid shopGuid, Guid userGuid)
        {
            var user = Users[userGuid];
            if (user == null) return;
            var shop = user.ShopsOwned.FirstOrDefault(lShop => lShop.ShopGuid.Equals(shopGuid));
            if (shop != null)
                user.ShopsOwned.Remove(shop);
        }

        public static int GetUsersCount()
        {
            return Users.Count;
        }

        public static void AddToUsersPurchaseHistory(ShoppingBag shoppingBag, Guid userGuid)
        {
            var user = Users[userGuid];
            if (user == null) return;
            user.PurchaseHistory.Add(shoppingBag);
        }

        /// <summary>
        /// returns wether or not the use purchased a specific product 
        /// </summary>
        /// <param name="shop"></param>
        /// <returns>returns trueif the user has purschased once in this Store , false otherwise</returns>
        public static bool HasPurchasedInShop(Shop shop, Guid userGuid)
        {
            var user = Users[userGuid];
            if (user == null) return false;
            foreach (ShoppingBag bag in user.PurchaseHistory)
            {
                if (bag.HasShop(shop))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// returns wether or not the user purchased a specific product 
        /// </summary>
        /// <param name="product"></param>
        /// <returns>true if purchased, flase othewrise</returns>
        public static bool HasPurchasedProduct(Product product, Guid userGuid)
        {
            var user = Users[userGuid];
            if (user == null) return false;
            foreach (ShoppingBag bag in user.PurchaseHistory)
            {
                if (bag.HasProduct(product))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool PurchaseBag(string username)
        {
            return true;
        }

        //TODO : REMOVE THIS, SHOULD NOT EXPOSE INTERNAL OBJECTS
        public static User GetUserByUsername(string username)
        {
            return Users.GetUserByUsername(username);
        }

        public static bool IsAdminUser(string username) => Users.Any(user => user.Username.Equals(username) && user.IsAdmin);
        public static bool ExistsAdminUser() => Users.Any(user => user.IsAdmin);

    }
}
