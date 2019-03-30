using DomainLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceLayer.Public_Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// Registers a user using the registration info.
        /// </summary>
        /// <param name="info">Info of the user to register</param>
        /// <returns>True if registered successfully. False otherwise, with an error message as out parameter.</returns>
        User Register(string username, string password, out string ErrorMessage);

        /// <summary>
        /// Logins a user using the username and password.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>True if it could login successfully. False otherwise.</returns>
        bool Login(string username, string password);

        /// <summary>
        /// Logs out the user.
        /// </summary>
        /// <param name="username"></param>
        bool Logout(string username);

        /// <summary>
        /// Opens a store for the user.
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns>True if opened successfully. False otherwise, with an error message as out parameter.</returns>
        Guid OpenShop(string username, out string errorMessage);

        /// <summary>
        /// Pay for the list of the products of the shop for the user.
        /// </summary>
        /// <param name="products"></param>
        /// <param name="userInfo"></param>
        /// <param name="sellingShop"></param>
        /// <returns>True if payed successfully. False otherwise with an error message as an out parameter.</returns>
        bool PurchaseBag(string username);
    }
}
