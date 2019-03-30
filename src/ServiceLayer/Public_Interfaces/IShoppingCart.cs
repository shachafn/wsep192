using DomainLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceLayer.Public_Interfaces
{
    interface IShoppingCartService
    {
        /// <summary>
        /// Adds the product to the shopping cart.
        /// </summary>
        /// <param name="product"></param>
        /// <param name="shoppingCart"></param>
        /// <param name="errorMessage"></param>
        /// <returns>True if added successfully. False otherwise, with an error message as an out parameter.</returns>
        bool AddProductToShoppingCart(Product product, ShoppingCart shoppingCart, out string errorMessage);

        /// <summary>
        /// Gets all the products on the user's cart.
        /// </summary>
        /// <param name="shoppingCart"></param>
        /// <returns>A enumerable collection of the products.</returns>
        IEnumerable<Product> GetAllProducts(ShoppingCart shoppingCart);

        /// <summary>
        /// Removes the product from the user's cart.
        /// </summary>
        /// <param name="product"></param>
        /// <param name="shoppingCart"></param>
        /// <param name="errorMessage"></param>
        /// <returns>True if removed successfully. False otherwise, with an error message as an out parameter.</returns>
        bool RemoveProduct(Product product, ShoppingCart shoppingCart, out string errorMessage);

        /// <summary>
        /// Set the amount of the product in the cart to the new amount.
        /// </summary>
        /// <param name="product"></param>
        /// <param name="shoppingCart"></param>
        /// <param name="newAmount"></param>
        /// <param name="errorMessage"></param>
        /// <returns>True if removed successfully. False otherwise, with an error message as an out parameter.</returns>
        bool ChangePurchasedProductAmount(Product product, ShoppingCart shoppingCart, int newAmount, out string errorMessage);
    }
}
