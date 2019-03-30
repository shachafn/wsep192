using System;
using System.Collections.Generic;
using System.Text;
using ServiceLayer.Services;

namespace ATBridge
{
    public class ProxyBridge : IBridge
    {
        private IBridge _real;

        public ProxyBridge()
        {
            _real = null;
        }

        public void SetRealBridge(IBridge impl)
        {
            if (_real == null)
                _real = impl;
        }

        /// <summary>
        /// Registers a user using the registration info.
        /// </summary>
        /// <param name="info">Info of the user to register</param>
        /// <returns>True if registered successfully. False otherwise, with an error message as out parameter.</returns>
        public bool Register(string username, string password)
        {
            return _real == null ? false : _real.Register(username, password);
        }

        /// <summary>
        /// Logins a user using the username and password.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>True if it could login successfully. False otherwise.</returns>
        public bool Login(string username, string password)
        {
            return _real == null ? false : _real.Login(username, password);
        }

        /// <summary>
        /// Logs out the user.
        /// </summary>
        /// <param name="username"></param>
        public bool Logout(string username)
        {
            return _real == null ? false : _real.Logout(username);
        }

        /// <summary>
        /// Opens a store for the user.
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns>True if opened successfully. False otherwise, with an error message as out parameter.</returns>
        public bool OpenShop(string username)
        {
            return _real == null ? false : _real.OpenShop(username);
        }

        /// <summary>
        /// Pay for the list of the products of the shop for the user.
        /// </summary>
        /// <param name="products"></param>
        /// <param name="userInfo"></param>
        /// <param name="sellingShop"></param>
        /// <returns>True if payed successfully. False otherwise with an error message as an out parameter.</returns>
        public bool PurchaseBag(string username)
        {
            return _real == null ? false : _real.PurchaseBag(username);
        }
    }
}
