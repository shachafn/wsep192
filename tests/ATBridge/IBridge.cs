using DomainLayer;
using System;
using System.Collections.Generic;

namespace ATBridge
{
    public interface IBridge
    {
        #region User Services
        /// <summary>
        /// Registers a user using the registration info.
        /// </summary>
        /// <param name="info">Info of the user to register</param>
        /// <returns>True if registered successfully. False otherwise, with an error message as out parameter.</returns>
        User Register(string username, string password);

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
        Guid OpenShop(string username);

        /// <summary>
        /// Pay for the list of the products of the shop for the user.
        /// </summary>
        /// <param name="products"></param>
        /// <param name="userInfo"></param>
        /// <param name="sellingShop"></param>
        /// <returns>True if payed successfully. False otherwise with an error message as an out parameter.</returns>
        bool PurchaseBag(string username);
        #endregion

        #region Shop Services
        /// <summary>
        /// Adds the product to the shop.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="category"></param>
        /// <param name="price"></param>
        /// <param name="quantity"></param>
        /// <param name="shopGuid"></param>
        /// <returns>True if added successfully. False otherwise.</returns>
        Guid AddProductToShop(string name, string category, double price, int quantity, Guid shopGuid);

        /// <summary>
        /// Removes the product from the shop.
        /// </summary>
        /// <param name="productGuid"></param>
        /// <param name="shopGuid"></param>
        /// <returns>True if added successfully. False otherwise.</returns>
        bool RemoveProductFromShop(Guid productGuid, Guid shopGuid);

        /// <summary>
        /// Set the price and quantity fields of the product to the new parameters.
        /// </summary>
        /// <param name="product"></param>
        /// <param name="price"></param>
        /// <param name="quantity"></param>
        /// <returns>True if added successfully. False otherwise.</returns>
        bool EditProduct(Guid shopGuid, Guid productGuid, double newPrice, int newQuantity);

        /// <summary>
        /// Returns a list of products with a name which contains the product name.
        /// </summary>
        /// <param name="productName"></param>
        /// <returns>A list of products.</returns>
        IEnumerable<Product> SearchProduct(Guid shopGuid, string productName);

        /// <summary>
        /// Adds the user as a shop owner.
        /// </summary>
        /// <param name="shopGuid"></param>
        /// <param name="ownerUsername"></param>
        /// <param name="managerToAddUsername"></param>
        /// <returns>True if added successfully. False otherwise.</returns>
        bool AddShopOwner(Guid shopGuid, string ownerUsername, string managerToAddUsername);

        /// <summary>
        /// Removes the shop owner from owning the shop and cascade delete all owners appointed by him.
        /// </summary>
        /// <param name="shopGuid"></param>
        /// <param name="ownerUsername"></param>
        /// <returns>True if the operation was done successfully.</returns>
        bool CascadeRemoveShopOwner(Guid shopGuid, string ownerUsername);

        /// <summary>
        /// Appoints the user as a shop manager of the shop.
        /// </summary>
        /// <param name="shopGuid"></param>
        /// <param name="ownerUsername"></param>
        /// <param name="managerToAddUsername"></param>
        /// <param name="priviliges"></param>
        /// <returns>True if the operation was done successfully.</returns>
        bool AddShopManager(Guid shopGuid, string ownerUsername, string managerToAddUsername, List<string> priviliges);

        /// <summary>
        /// Removes the user from managing the shop.
        /// </summary>
        /// <param name="shopGuid"></param>
        /// <param name="ownerUsername"></param>
        /// <returns></returns>
        bool RemoveShopManager(Guid shopGuid, string ownerUsername);
        #endregion

        #region Shopping Cart Services
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
        #endregion
    }
}
