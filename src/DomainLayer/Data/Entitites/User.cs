using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLayer.Data.Entitites
{
    public class User : BaseEntity
    {
        public bool IsLoggedIn { get; set; }
        public string Username { get; private set; }
        public bool IsAdmin { get; private set; }

        private string _passHash;
        public ShoppingBag CurrentBag { get; set; }
        public List<ShoppingBag> PurchaseHistory { get; set; }
        private UserInfo _userInfo;
        public List<Shop> ShopsOwned { get; set; }
        private bool isAdmin;
        // Constructor that takes no arguments:
        public User() : base()
        {
            IsLoggedIn = false;
            Username = "";
            _passHash = "";
            CurrentBag = new ShoppingBag();
            PurchaseHistory = new List<ShoppingBag>();
            _userInfo = new UserInfo();
            ShopsOwned = new List<Shop>();
            IsAdmin = false;
        }

        public User(string username, string password, bool isAdmin) : base()
        {
            Username = username;
            _passHash = GetStringSha256Hash(password);
            IsLoggedIn = false;
            CurrentBag = new ShoppingBag();
            PurchaseHistory = new List<ShoppingBag>();
            _userInfo = new UserInfo();
            ShopsOwned = new List<Shop>();
            IsAdmin = isAdmin;
        }


        internal List<ShoppingBag> GetShoppingHistory()
        {
            return PurchaseHistory;
        }


        public bool CheckPass(string password)
        {
            return _passHash.Equals(GetStringSha256Hash(password));
        }

        /// <summary>
        /// A method that is used to hash the password
        /// </summary>
        /// <param name="password"></param>
        /// <returns>Hashed version of the password</returns>
        internal static string GetStringSha256Hash(string password)
        {
            if (string.IsNullOrEmpty(password))
                return string.Empty;

            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] textData = System.Text.Encoding.UTF8.GetBytes(password);
                byte[] hash = sha.ComputeHash(textData);
                return BitConverter.ToString(hash).Replace("-", string.Empty);
            }
        }
        // Method that overrides the base class (System.Object) implementation.
        public override string ToString()
        {
            return "User:\n username: " + Username + "\n" + this._userInfo.ToString() + "\n" + "logged: " + IsLoggedIn
                + "\ncurrent shopping bag:" + CurrentBag.ToString() + "\n is admin: " + IsAdmin;
        }

        public void PurchaseBag()
        {
            //TODO: IMPLEMENT
        }
    }
}
