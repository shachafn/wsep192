using DomainLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceLayer.Public_Interfaces
{
    interface IShopService
    {
        /// <summary>
        /// Adds the product to the shop.
        /// </summary>
        /// <param name="product"></param>
        /// <param name="shop"></param>
        /// <param name="errorMessage"></param>
        /// <returns>True if added successfully. False otherwise, with an error message as an out parameter.</returns>
        bool AddProductToShop(Product product, DomainLayer.Shop shop, out string errorMessage);

        /// <summary>
        /// Removes the product from the shop.
        /// </summary>
        /// <param name="product"></param>
        /// <param name="shop"></param>
        /// <param name="errorMessage"></param>
        /// <returns>True if added successfully. False otherwise, with an error message as an out parameter.</returns>
        bool RemoveProductFromShop(Product product, DomainLayer.Shop shop, out string errorMessage);

        /// <summary>
        /// Set the price and quantity fields of the product to the new parameters.
        /// </summary>
        /// <param name="product"></param>
        /// <param name="price"></param>
        /// <param name="quantity"></param>
        /// <param name="errorMessage"></param>
        /// <returns>True if added successfully. False otherwise, with an error message as an out parameter.</returns>
        bool EditProduct(Product product, double newPrice, int newQuantity, out string errorMessage);

        /// <summary>
        /// Returns a list of products with a name which contains the product name.
        /// </summary>
        /// <param name="productName"></param>
        /// <returns>A list of products.</returns>
        IEnumerable<Product> SearchProduct(string productName);

        /// <summary>
        /// Adds the user as a shop owner.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="errorMessage"></param>
        /// <returns>True if added successfully. False otherwise, with an error message as an out parameter.</returns>
        bool AddShopOwner(string username, out string errorMessage);

        /// <summary>
        /// Removes the shop owner from owning the shop and cascade delete all owners appointed by him.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="shop"></param>
        /// <param name="errorMessage"></param>
        /// <returns>True if the operation was done successfully. False otherwise, with an error message as an out parameter.</returns>
        bool CascadeRemoveShopOwner(string username, Shop shop, out string errorMessage);

        /// <summary>
        /// Appoints the user as a shop manager of the shop.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="errorMessage"></param>
        /// <param name="shop"></param>
        /// <returns>True if the operation was done successfully. False otherwise, with an error message as an out parameter.</returns>
        bool AddShopManager(string username, Shop shop, out string errorMessage);

        /// <summary>
        /// Removes the user from managing the shop.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="errorMessage"></param>
        /// <param name="shop"></param>
        /// <returns></returns>
        bool RemoveShopManager(string username, Shop shop, out string errorMessage);
    }
}
