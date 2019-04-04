using System;
using System.Collections.Generic;

namespace DomainLayer.Facade
{
    /// <summary>
    /// Exposes the Domain Layer's functionality, should not expose internal structures.
    /// If really needed, can expose new structures here and then return them.
    /// </summary>
    public interface IDomainLayerFacade
    {
        /// <summary>
        /// Registers a user with the given parameters.
        /// </summary>
        /// <returns>Guid of the user. Guid.Empty on error.</returns>
        bool Register(Guid userGuid, string username, string password);

        /// <summary>
        /// Logging in the user.
        /// </summary>
        /// <returns>Guid of the user, Guid.Empty on error.</returns>
        Guid Login(Guid userGuid, string username, string password);

        /// <summary>
        /// Logging out the user.
        /// </summary>
        /// <param name="userGuid"></param>
        /// <returns>True if logged out successfully. False otherwise.</returns>
        bool Logout(Guid userGuid);

        /// <summary>
        /// Opens a shop for the user.
        /// </summary>
        /// <param name="userGuid"></param>
        /// <returns>Guid of the shop. On error - Guid.Empty</returns>
        Guid OpenShop(Guid userGuid);

        /// <summary>
        /// Purchases the user's cart of the given shop.
        /// </summary>
        /// <param name="userGuid"></param>
        /// <param name="shopGuid"></param>
        /// <returns>True if purchased successfully. False otherwise.</returns>
        bool PurchaseCart(Guid userGuid, Guid shopGuid);

        /// <summary>
        /// Initializes the system and creates an admin user if provided.
        /// </summary>
        /// <returns>True if operation succeeded. False otherwise. </returns>
        bool Initialize(Guid userGuid, string username, string password);


        bool ConnectToPaymentSystem(Guid userGuid);
        bool ConnectToSupplySystem(Guid userGuid);

        Guid AddShopProduct(Guid userGuid, Guid shopGuid, string name, string category, double price, int quantity);
        bool EditShopProduct(Guid userGuid, Guid shopGuid, Guid productGuid, double newPrice, int newQuantity);
        bool RemoveShopProduct(Guid userGuid, Guid shopGuid, Guid shopProductGuid);
        bool AddProductToShoppingCart(Guid userGuid, Guid shopGuid, Guid shopProductGuid, int quantity);
        bool AddShopManager(Guid userGuid, Guid shopGuid, Guid newManagaerGuid, List<string> priviliges);
        bool CascadeRemoveShopOwner(Guid userGuid, Guid shopGuid, Guid ownerToRemoveGuid);
        bool EditProductInCart(Guid userGuid, Guid shopGuid, Guid shopProductGuid, int newAmount);
        bool RemoveProductFromCart(Guid userGuid, Guid shopGuid, Guid shopProductGuid);
        ICollection<Guid> GetAllProductsInCart(Guid userGuid, Guid shopGuid);
        bool RemoveUser(Guid userGuid, Guid userToRemoveGuid);
        ICollection<Guid> SearchProduct(Guid userGuid, Guid shopGuid, string productName);
        bool RemoveShopManager(Guid userGuid, Guid shopGuid, Guid managerToRemoveGuid);
    }
}
