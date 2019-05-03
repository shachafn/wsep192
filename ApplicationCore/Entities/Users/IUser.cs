using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities.Users
{
    public interface IUser
    {
        #region Properties
        Guid Guid { get; }
        bool IsAdmin { get; }
        #endregion

        #region Public Methods
        bool AddProductToCart(Guid shopGuid, Guid shopProductGuid, int quantity);
        Guid AddProductToShop(Guid shopGuid, string name, string category, double price, int quantity);
        bool AddShopManager(Guid shopGuid, Guid newManagaerGuid, List<string> priviliges);
        bool AddShopOwner(Guid shopGuid, Guid newManagaerGuid);
        bool CascadeRemoveShopOwner(Guid shopGuid, Guid ownerToRemoveGuid);
        bool ConnectToPaymentSystem();
        bool ConnectToSupplySystem();
        bool EditProductInCart(Guid shopGuid, Guid shopProductGuid, int newAmount);
        void EditProductInShop(Guid shopGuid, Guid productGuid, double newPrice, int newQuantity);
        ICollection<Guid> GetAllProductsInCart(Guid shopGuid);
        ICollection<Guid> GetShoppingHistory();
        Guid OpenShop();
        bool PurchaseBag();
        bool RemoveProductFromCart(Guid shopGuid, Guid shopProductGuid);
        bool RemoveProductFromShop(Guid shopGuid, Guid shopProductGuid);
        bool RemoveShopManager(Guid shopGuid, Guid managerToRemoveGuid);
        bool RemoveUser(Guid userToRemoveGuid);
        ICollection<Guid> SearchProduct(ICollection<string> toMatch, string searchType);
        bool SetState(IAbstractUserState newState);
        bool PurchaseCart(Guid guid, Guid shopGuid);
        #endregion
    }
}