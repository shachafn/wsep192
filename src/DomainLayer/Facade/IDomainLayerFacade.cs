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
        bool Register(Guid userGuid, string username, string password);
        Guid Login(Guid userGuid, string username, string password);
        bool Logout(Guid userGuid);
        Guid OpenShop(Guid userGuid);
        bool PurchaseBag(Guid userGuid);
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
        bool ChangeUserState(Guid userGuid, string newState);
    }
}
