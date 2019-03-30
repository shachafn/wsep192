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
    }
}
