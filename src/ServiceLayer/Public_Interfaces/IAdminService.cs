using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceLayer.Public_Interfaces
{
    public interface IAdminService
    {
        /// <summary>
        /// Initializes the system.
        /// </summary>
        ///     <constraints>
        ///     1. All external services should be connectable.
        ///     2. Admin registration info should be provided and be valid if no Admin is registered.
        ///     </constraints>
        /// <returns>True if the system initialized successfully. False otherwise.</returns>
        bool Initialize(string username = null, string password = null);

        /// <summary>
        /// Removes the user.
        /// </summary>
        ///     <constraints>
        ///     1. User must not be the only owner of an active shop.
        ///     </constraints>
        /// <param name="userInfo"></param>
        /// <returns>True if the user was removed successfully. False otherwise.</returns>
        bool RemoveUser(UserInfo userInfo);

        /// <summary>
        /// Checks if the system can connect to the payment system.
        /// </summary>
        /// <returns>True if can connect, false otherwise.</returns>
        bool ConnectToPaymentSystem();

        /// <summary>
        /// Checks if the system can connect to the payment system.
        /// </summary>
        /// <returns>True if can connect, false otherwise.</returns>
        bool ConnectToSupplySystem();
    }
}
