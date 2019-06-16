using ApplicationCore.Entitites;
using System;
using System.Collections.Generic;
using DomainLayer.Policies;

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
        bool AddShopManager(Guid shopGuid, Guid newManagaerGuid, List<bool> privileges);
        bool AddShopOwner(Guid shopGuid, Guid newManagaerGuid);
        bool CascadeRemoveShopOwner(Guid shopGuid, Guid ownerToRemoveGuid);
        bool EditProductInCart(Guid shopGuid, Guid shopProductGuid, int newAmount);
        void EditProductInShop(Guid shopGuid, Guid productGuid, double newPrice, int newQuantity);
        ICollection<ShopProduct> GetAllProductsInCart(Guid shopGuid);
        Guid OpenShop();
        Guid OpenShop(string name);
        void ReopenShop(Guid shopGuid);
        void CloseShop(Guid shopGuid);
        void CloseShopPermanently(Guid shopGuid);
        bool PurchaseCart(Guid shopGuid);
        double GetCartPrice(Guid shopGuid);
        bool RemoveProductFromCart(Guid shopGuid, Guid shopProductGuid);
        bool RemoveProductFromShop(Guid shopGuid, Guid shopProductGuid);
        bool RemoveShopManager(Guid shopGuid, Guid managerToRemoveGuid);
        ICollection<Tuple<ShopProduct, Guid>> SearchProduct(ICollection<string> toMatch, string searchType);
        Guid AddNewPurchasePolicy(Guid userGuid , Guid shopGuid, IPurchasePolicy newPolicy);
        Guid AddNewDiscountPolicy(Guid userGuid ,Guid shopGuid, IDiscountPolicy newPolicy);
        ICollection<Tuple<Guid, ShopProduct, int>> GetPurchaseHistory();
        #endregion
    }
}