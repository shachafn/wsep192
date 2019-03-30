using System;
using System.Collections.Generic;

namespace DomainLayer
{
    public class User
    {
        public static Dictionary<string, User> users = new Dictionary<string, User>(); // a list of all registered users in the current session

        private bool _logged;
        public string Username { get => Username; private set => Username=value; }
        public bool IsAdmin { get => isAdmin; private set => isAdmin = value; }

        private string _passHash;
        public ShoppingBag CurrentBag { get; set; }
        private List<ShoppingBag> _purchaseHistory;
        private UserInfo _userInfo;
        private List<Shop> _shopsOwned;
        private bool isAdmin; 
        // Constructor that takes no arguments:
        public User()
        {
            _logged = false;
            Username = "";
            _passHash = "";
            CurrentBag = new ShoppingBag();
            _purchaseHistory = new List<ShoppingBag>();
            _userInfo = new UserInfo();
            _shopsOwned = new List<Shop>();
            IsAdmin = false;
        }

        public User(string username, string password)
        {
            Username = username;
            _passHash = GetStringSha256Hash(password);
            _logged = false;
            CurrentBag = new ShoppingBag();
            _purchaseHistory = new List<ShoppingBag>();
            _userInfo = new UserInfo();
            _shopsOwned = new List<Shop>();
            IsAdmin = false;
        }

        public bool IsLogged()
        {
            return _logged;
        }

        internal List<ShoppingBag> GetShoppingHistory()
        {
           return _purchaseHistory;
        }

        public static User GetUserByUsername(string username) => users[username];

        /// <summary>
        /// logs the user in , changes its logged field to true , and retrieves all it's stored information
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>true if the login was sucseesful, false if one or more of the parameters were wrong 
        /// or if the user is already connected</returns>
        public bool Login(string username, string password)
        {
            // check if the user exist and the password is correct 
            if (!users.ContainsKey(username) || users[username].CheckPass(password) || users[username]._logged)
            {
                return false;
            }

            _logged = true;
            CopyUserData(users[username]);// used to recive all of the data saved on the user
            return true;
        }

        /** 
         *<summary>this method logs the user out , and saves it's changed properties</summary> 
         * <returns>false if the user is not logged in , true otherwise</returns> 
         */
        public bool Logout()
        {
            if (!_logged)
            {
                return false;
            }
            SaveUserChanges();
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
            if (users.ContainsKey(username) && password.Length > 5)
            {
                return null;
            }
            User newUser = new User(username, password);
            users.Add(username, newUser);
            return newUser;
        }
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
        public Guid OpenShop()
        {
            if(!_logged)
            {
                return Guid.Empty;
            }
            Shop shop = new Shop();
            _shopsOwned.Add(shop);
            return shop.ShopGuid;
        }

        private bool CheckPass(string password)
        {
            return _passHash.Equals(GetStringSha256Hash(password));
        }

        internal void RemoveShop(Shop shop)
        {
            _shopsOwned.Remove(shop);
        }

        /*
         * this method should copy all the fields from @user to this instance.
         */
        private void CopyUserData(User user)
        {
            Username = user.Username;
            _passHash = user._passHash;
            CurrentBag = user.CurrentBag;
            _purchaseHistory = user._purchaseHistory;
            _userInfo = user._userInfo;
            _shopsOwned = user._shopsOwned;
        }
        /// <summary>
        /// returns wether or not the use purchased a specific product 
        /// </summary>
        /// <param name="shop"></param>
        /// <returns>returns trueif the user has purschased once in this Store , false otherwise</returns>
        public bool HasPurchasedInShop(Shop shop)
        {
            foreach(ShoppingBag  bag in _purchaseHistory)
            {
                if(bag.HasShop(shop))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// returns wether or not the use purchased a specific product 
        /// </summary>
        /// <param name="product"></param>
        /// <returns>true if purchased, flase othewrise</returns>
        public bool HasPurchasedProduct(Product product)
        {
            foreach (ShoppingBag bag in _purchaseHistory)
            {
                if (bag.HasProduct(product))
                {
                    return true;
                }
            }
            return false;
        }
        private void SaveUserChanges()
        {
            User savedUser = users[Username];
            savedUser.CurrentBag = CurrentBag;
            savedUser._purchaseHistory = _purchaseHistory;
            savedUser._userInfo = _userInfo;
            savedUser._shopsOwned = _shopsOwned;
        }
        //a method that is used to hash the password
        internal static string GetStringSha256Hash(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] textData = System.Text.Encoding.UTF8.GetBytes(text);
                byte[] hash = sha.ComputeHash(textData);
                return BitConverter.ToString(hash).Replace("-", string.Empty);
            }
        }
        // Method that overrides the base class (System.Object) implementation.
        public override string ToString()
        {
            return "User:\n username: "+Username+"\n"+this._userInfo.ToString()+"\n"+"logged: "+_logged
                + "\ncurrent shopping bag:"+CurrentBag.ToString()+"\n is admin: "+IsAdmin;
        }

        public void PurchaseBag() { }

    }
}

