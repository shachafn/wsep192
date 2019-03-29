using System;
using System.Collections.Generic;

namespace DomainLayer
{
    public class User
    {
        public static Dictionary<string, User> users = new Dictionary<string, User>(); // a list of all registered users in the current session

        private bool logged;
        private string username;
        private string passHash;
        private ShoppingBag currentBag;
        private List<ShoppingBag> purchaseHistory;
        private UserInfo userInfo;

        // Constructor that takes no arguments:
        public User()
        {
            logged = false;
        }

        public User(string username, string password)
        {
            this.username = username;
            this.passHash = GetStringSha256Hash(password);
        }

        internal List<ShoppingBag> getShoppingHistory()
        {
           return this.purchaseHistory;
        }

        bool login(string username, string password)
        {
            // check if the user exist and the password is correct 
            if (!users.ContainsKey(username) || users[username].checkPass(password))
            {
                return false;
            }

            logged = true;
            copyUserData(users[username]);// used to recive all of the data saved on the user
            return true;
        }

        /* 
         * this method logs the user out , and saves it's changed properties
         * returns : false if the user is not logged in , true otherwise
         */
        bool logout()
        {
            if (!logged)
            {
                return false;
            }
            saveUserChanges();
            return true;
        }


        /*
         * this method recieves : a  username and  a password 
         * if the username is not taken already , the method creates a new user with this credentials and stores it in the users list
         * returns the created user or null otherwise
         */
        public static User Register(string username, string password)
        {
            if (users.ContainsKey(username))
            {
                return null;
            }
            User newUser = new User(username, password);
            users.Add(username, newUser);
            return newUser;
        }
        List<Product> search(string searchString, List<ProductFilter> filters = null)
        {
            List<Product> productsFound = new List<Product>();
            foreach (Shop shop in shops)
            {
                productsFound.AddRange(shop.search(searchString));
            }
            foreach (ProductFilter filter in filters)
            {
                filter.apply(productsFound);
            }
            return productsFound;
        }
        void openShop()
        {

        }

        private bool checkPass(string password)
        {
            return this.passHash.Equals(GetStringSha256Hash(password));
        }

        /*
         * this method should copy all the fields from @user to this instance.
         */
        private void copyUserData(User user)
        {
            this.username = user.username;
            this.passHash = user.passHash;
            this.currentBag = user.currentBag;
            this.purchaseHistory = user.purchaseHistory;
            this.userInfo = user.userInfo;
        }

        private void saveUserChanges()
        {
            User savedUser = users[username];
            savedUser.currentBag = this.currentBag;
            savedUser.purchaseHistory = this.purchaseHistory;
            savedUser.userInfo = this.userInfo;
        }
        //a method that is used to hash the password
        internal static string GetStringSha256Hash(string text)
        {
            if (String.IsNullOrEmpty(text))
                return String.Empty;

            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] textData = System.Text.Encoding.UTF8.GetBytes(text);
                byte[] hash = sha.ComputeHash(textData);
                return BitConverter.ToString(hash).Replace("-", String.Empty);
            }
        }
        // Method that overrides the base class (System.Object) implementation.
        public override string ToString()
        {
            return "";
        }
    }
}

