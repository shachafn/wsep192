using System;
using System.Collections.Generic;

namespace DomainLayer.Data.Entitites.Users
{
    public abstract class AbstractUserState : BaseUser, IUser
	{
		public AbstractUserState(string username, string password) : base (username,password) { }

        public abstract ICollection<ShoppingBag> GetShoppingHistory();
        public abstract Guid OpenShop();
        public abstract void PurchaseBag();
        public abstract bool RemoveUser(Guid userToRemoveGuid);
        public abstract bool ConnectToPaymentSystem();
        public abstract bool ConnectToSupplySystem();
        public abstract Guid AddShopProduct(Guid shopGuid, string name, string category, double price, int quantity);
        public abstract void EditShopProduct(Guid shopGuid, Guid productGuid, double newPrice, int newQuantity);
        public abstract bool RemoveShopProduct(Guid shopGuid, Guid shopProductGuid);
        public abstract bool AddProductToShoppingCart(Guid shopGuid, Guid shopProductGuid, int quantity);
        public abstract bool AddShopManager(Guid shopGuid, Guid newManagaerGuid, List<string> priviliges);
        public abstract bool CascadeRemoveShopOwner(Guid shopGuid, Guid ownerToRemoveGuid);
        public abstract bool EditProductInCart(Guid shopGuid, Guid shopProductGuid, int newAmount);
        public abstract bool RemoveProductFromCart(Guid shopGuid, Guid shopProductGuid);
        public abstract ICollection<Guid> GetAllProductsInCart(Guid shopGuid);
        public abstract bool RemoveShopManager(Guid shopGuid, Guid managerToRemoveGuid);
        public abstract bool AddShopOwner(Guid shopGuid, Guid newManagaerGuid);
    }
}
