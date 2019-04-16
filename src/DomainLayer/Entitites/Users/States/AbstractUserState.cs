using System;
using System.Collections.Generic;

namespace DomainLayer.Data.Entitites.Users.States
{
    public abstract class AbstractUserState
	{
        public abstract ICollection<ShoppingBag> GetShoppingHistory();
        public abstract Guid OpenShop(BaseUser baseUser);
        public abstract bool PurchaseBag();
        public abstract bool RemoveUser(Guid userToRemoveGuid);
        public abstract bool ConnectToPaymentSystem();
        public abstract bool ConnectToSupplySystem();
        public abstract Guid AddShopProduct(BaseUser baseUser, Guid shopGuid, string name, string category, double price, int quantity);
        public abstract void EditShopProduct(BaseUser baseUser, Guid shopGuid, Guid productGuid, double newPrice, int newQuantity);
        public abstract bool RemoveShopProduct(BaseUser baseUser, Guid shopGuid, Guid shopProductGuid);
        public abstract bool AddShopManager(BaseUser baseUser, Guid shopGuid, Guid newManagaerGuid, List<string> priviliges);
        public abstract bool CascadeRemoveShopOwner(BaseUser baseUser, Guid shopGuid, Guid ownerToRemoveGuid);
        public abstract bool AddProductToShoppingCart(BaseUser baseUser, Guid shopGuid, Guid shopProductGuid, int quantity);
        public abstract bool EditProductInCart(BaseUser baseUser, Guid shopGuid, Guid shopProductGuid, int newAmount);
        public abstract bool RemoveProductFromCart(BaseUser baseUser, Guid shopGuid, Guid shopProductGuid);
        public abstract ICollection<Guid> GetAllProductsInCart(BaseUser baseUser, Guid shopGuid);
        public abstract bool RemoveShopManager(BaseUser baseUser, Guid shopGuid, Guid managerToRemoveGuid);
        public abstract bool AddShopOwner(BaseUser baseUser, Guid shopGuid, Guid newManagaerGuid);
    }
}
