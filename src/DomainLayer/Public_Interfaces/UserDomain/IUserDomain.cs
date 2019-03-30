using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace DomainLayer.Public_Interfaces.UserDomain
{
    interface IUserDomain
    {
        /// <summary>
        /// Registers a user using the registration info.
        /// </summary>
        /// <param name="info">Info of the user to register</param>
        /// <returns>True if registered successfully. False otherwise, with an error message as out parameter.</returns>
        bool RegisterUser(UserRegistrationInfo info, out string ErrorMessage);

        /// <summary>
        /// Logins a user using the username and password.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>True if it could login successfully. False otherwise.</returns>
        bool Login(string username, string password);

        /// <summary>
        /// Logins a user using the mailAddress and password.
        /// </summary>
        /// <param name="mailAddress"></param>
        /// <param name="password"></param>
        /// <returns>True if it could login successfully. False otherwise.</returns>
        bool Login(MailAddress mailAddress, string password);

        /// <summary>
        /// Logs out the user.
        /// </summary>
        /// <param name="username"></param>
        void Logout(string username);

        /// <summary>
        /// Opens a store for the user.
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns>True if opened successfully. False otherwise, with an error message as out parameter.</returns>
        bool OpenStore(out string errorMessage);

        /// <summary>
        /// Returns a list of all product purchases info.
        /// </summary>
        /// <param name="username"></param>
        /// <returns>The list.</returns>
        IEnumerable<ProductPurchaseHistory> WatchHistory(string username);

        /// <summary>
        /// Replaces the user's profile info with a new profile info.
        /// </summary>
        /// <param name="newProfileInfo"></param>
        /// <param name="errorMessage"></param>
        /// <returns>True if replaced successfully. False otherwise, with an error message as an out parameter.</returns>
        bool EditProfile(ProfileInfo newProfileInfo, out string errorMessage);
    }
}