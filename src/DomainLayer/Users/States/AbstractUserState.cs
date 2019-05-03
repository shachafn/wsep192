using ApplicationCore.Entities.Users;
using System;
using System.Collections.Generic;

namespace DomainLayer.Users.States
{
    public abstract class AbstractUserState : IAbstractUserState
    {
        public abstract ICollection<Guid> GetShoppingHistory();
        public abstract Guid OpenShop(BaseUser baseUser);
        public abstract bool PurchaseBag();
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
        public abstract ICollection<Guid> GetAllProductsInCart(BaseUser baseUser, Guid shopGuid);
        public abstract bool RemoveShopManager(BaseUser baseUser, Guid shopGuid, Guid managerToRemoveGuid);
        public abstract bool AddShopOwner(BaseUser baseUser, Guid shopGuid, Guid newManagaerGuid);
        public abstract ICollection<Guid> SearchProduct(ICollection<string> toMatch, string searchType);
        public abstract bool PurchaseCart(Guid userGuid,Guid shopGuid);
    }
}
