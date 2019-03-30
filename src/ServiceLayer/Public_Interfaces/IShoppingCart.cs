using DomainLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceLayer.Public_Interfaces
{
    public interface IShoppingCartService
    {
        /// <summary>
        /// Adds the product to the shopping cart.
        /// </summary>
        /// <param name="productGuid"></param>
        /// <param name="shopOfCartGuid"></param>
        /// <param name="username"></param>
        /// <returns>True if added successfully. False otherwise.</returns>
        bool AddProductToShoppingCart(Guid productGuid, Guid shopOfCartGuid, string username);

        /// <summary>
        /// Gets all the products on the user's cart.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="shopOfCartGuid"></param>
        /// <returns>An enumerable collection of the products.</returns>
        IEnumerable<Guid> GetAllProducts(string username, Guid shopOfCartGuid);

        /// <summary>
        /// Removes the product from the user's cart.
        /// </summary>
        /// <param name="productGuid"></param>
        /// <param name="shopOfCartGuid"></param>
        /// <param name="username"></param>
        /// <returns>True if removed successfully. False otherwise.</returns>
        bool RemoveProduct(Guid productGuid, Guid shopOfCartGuid, string username);

        /// <summary>
        /// Sets the amount of the product in the cart to the new amount.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="shopOfCartGuid"></param>
        /// <param name="productGuid"></param>
        /// <param name="newAmount"></param>
        /// <returns>True if removed successfully. False otherwise.</returns>
        bool ChangePurchasedProductAmount(string username, Guid shopOfCartGuid, Guid productGuid, int newAmount);
    }
}
