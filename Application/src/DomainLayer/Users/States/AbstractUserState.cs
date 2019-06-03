using ApplicationCore.Entities.Users;
using DomainLayer.Policies;
using ApplicationCore.Entitites;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace DomainLayer.Users.States
{
    public abstract class AbstractUserState : IAbstractUserState
    {
        //private ILogger<AbstractUserState> _logger;
        public abstract ICollection<Guid> GetShoppingHistory();
        public abstract Guid OpenShop(BaseUser baseUser);
        public abstract Guid OpenShop(BaseUser baseUser, string shopName);
        public abstract void ReopenShop(Guid shopGuid);
        public abstract void CloseShop(Guid shopGuid);
        public abstract void CloseShopPermanently(Guid shopGuid);
        public abstract bool PurchaseCart(BaseUser baseUser, Guid shopGuid);
        public abstract bool RemoveUser(Guid userToRemoveGuid);
        public abstract bool ConnectToPaymentSystem();
        public abstract bool ConnectToSupplySystem();
        public abstract Guid AddProductToShop(BaseUser baseUser, Guid shopGuid, string name, string category, double price, int quantity);
        public abstract void EditProductInShop(BaseUser baseUser, Guid shopGuid, Guid productGuid, double newPrice, int newQuantity);
        public abstract bool RemoveProductFromShop(BaseUser baseUser, Guid shopGuid, Guid shopProductGuid);
        public abstract bool AddShopManager(BaseUser baseUser, Guid shopGuid, Guid newManagaerGuid, List<string> priviliges);
        public abstract bool CascadeRemoveShopOwner(BaseUser baseUser, Guid shopGuid, Guid ownerToRemoveGuid);
        public abstract bool AddProductToCart(BaseUser baseUser, Guid shopGuid, Guid shopProductGuid, int quantity);
        public abstract bool EditProductInCart(BaseUser baseUser, Guid shopGuid, Guid shopProductGuid, int newAmount);
        public abstract bool RemoveProductFromCart(BaseUser baseUser, Guid shopGuid, Guid shopProductGuid);
        public abstract ICollection<ShopProduct> GetAllProductsInCart(BaseUser baseUser, Guid shopGuid);
        public abstract bool RemoveShopManager(BaseUser baseUser, Guid shopGuid, Guid managerToRemoveGuid);
        public abstract bool AddShopOwner(BaseUser baseUser, Guid shopGuid, Guid newManagaerGuid);
        public abstract Guid AddNewPurchasePolicy(Guid userGuid, Guid shopGuid, IPurchasePolicy newPolicy);
        public abstract Guid AddNewDiscountPolicy(Guid userGuid, Guid shopGuid, IDiscountPolicy newPolicy);
        public abstract ICollection<Tuple<ShopProduct, Guid>> SearchProduct(ICollection<string> toMatch, string searchType);

    }
}
