using DomainLayer.Policies;
using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities.Users
{
    public interface IAbstractUserState
    {
        bool AddProductToCart(BaseUser baseUser, Guid shopGuid, Guid shopProductGuid, int quantity);
        Guid AddProductToShop(BaseUser baseUser, Guid shopGuid, string name, string category, double price, int quantity);
        bool AddShopManager(BaseUser baseUser, Guid shopGuid, Guid newManagaerGuid, List<string> priviliges);
        bool AddShopOwner(BaseUser baseUser, Guid shopGuid, Guid newManagaerGuid);
        bool CascadeRemoveShopOwner(BaseUser baseUser, Guid shopGuid, Guid ownerToRemoveGuid);
        bool ConnectToPaymentSystem();
        bool ConnectToSupplySystem();
        bool EditProductInCart(BaseUser baseUser, Guid shopGuid, Guid shopProductGuid, int newAmount);
        void EditProductInShop(BaseUser baseUser, Guid shopGuid, Guid productGuid, double newPrice, int newQuantity);
        ICollection<Guid> GetAllProductsInCart(BaseUser baseUser, Guid shopGuid);
        ICollection<Guid> GetShoppingHistory();
        Guid OpenShop(BaseUser baseUser);
        bool PurchaseBag();
        bool RemoveProductFromCart(BaseUser baseUser, Guid shopGuid, Guid shopProductGuid);
        bool RemoveProductFromShop(BaseUser baseUser, Guid shopGuid, Guid shopProductGuid);
        bool RemoveShopManager(BaseUser baseUser, Guid shopGuid, Guid managerToRemoveGuid);
        bool RemoveUser(Guid userToRemoveGuid);
        ICollection<Guid> SearchProduct(ICollection<string> toMatch, string searchType);
        bool PurchaseCart(Guid userGuid, Guid shopGuid);
        bool AddNewPurchasePolicy(Guid userGuid, Guid shopGuid, IPurchasePolicy newPolicy);
        bool AddNewDiscountPolicy(Guid userGuid, Guid shopGuid, IDiscountPolicy newPolicy);
    }
}