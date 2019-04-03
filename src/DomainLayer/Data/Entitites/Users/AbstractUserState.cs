using System;
using System.Collections.Generic;

namespace DomainLayer.Data.Entitites.Users
{
    public abstract class AbstractUserState : BaseUser, IUser
	{
		public AbstractUserState(string username, string password) : base (username,password) { }

        public abstract bool AddShopOwner(Guid shopGuid, Guid userGuid);
        public abstract ICollection<ShoppingBag> GetShoppingHistory();
        public abstract Guid OpenShop();
        public abstract void PurchaseBag();
        public abstract bool RemoveUser(Guid userToRemoveGuid);
        public abstract bool ConnectToPaymentSystem();
        public abstract bool ConnectToSupplySystem();
        public abstract void AddShopProduct(Guid shopGuid, string name, string category, double price, int quantity);
        public abstract void EditShopProduct(Guid shopGuid, Guid productGuid, double newPrice, int newQuantity);
    }
}
