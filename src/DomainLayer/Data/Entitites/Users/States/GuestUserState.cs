using DomainLayer.Exceptions;
using System;
using System.Collections.Generic;

namespace DomainLayer.Data.Entitites.Users.States
{
    public class GuestUserState : AbstractUserState
    {
        public GuestUserState(string username, string password) : base(username, password) { }


        public override bool AddShopOwner(Guid shopGuid, Guid userGuid)
        {
            throw new BadStateException($"Tried to invoke AddShopOwner in Guest State");
        }

        public override ICollection<ShoppingBag> GetShoppingHistory()
        {
            throw new BadStateException($"Tried to invoke GetShoppingHistory in Guest State");
        }

        public override Guid OpenShop()
        {
            throw new BadStateException($"Tried to invoke OpenShop in Guest State");
        }

        public override void PurchaseBag()
        {
            throw new BadStateException($"Tried to invoke PurchaseBag in Guest State");
        }

        public override bool RemoveUser(Guid userToRemoveGuid)
        {
            throw new BadStateException($"Tried to invoke RemoveUser in Guest State");
        }

        public override bool ConnectToPaymentSystem()
        {
            throw new BadStateException($"Tried to invoke ConnectToPaymentSystem in Guest State");
        }

        public override bool ConnectToSupplySystem()
        {
            throw new BadStateException($"Tried to invoke ConnectToSupplySystem in Guest State");
        }

        public override void AddShopProduct(Guid shopGuid, string name, string category, double price, int quantity)
        {
            throw new BadStateException($"Tried to invoke AddShopProduct in Guest State");
        }

        public override void EditShopProduct(Guid shopGuid, Guid productGuid, double newPrice, int newQuantity)
        {
            throw new BadStateException($"Tried to invoke EditShopProduct in Guest State");
        }
    }
}
