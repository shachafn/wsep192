using DomainLayer.Exceptions;
using System;
using System.Collections.Generic;

namespace DomainLayer.Data.Entitites.Users.States
{
    public class BuyerUserState : AbstractUserState
    {
        public ICollection<ShoppingBag> PurchaseHistory { get; set; }
        public ShoppingBag CurrentBag { get; set; }
        public BuyerUserState(string username, string password) : base(username, password) { }


        public override ICollection<ShoppingBag> GetShoppingHistory()
        {
            throw new NotImplementedException();
        }

        public override void PurchaseBag()
        {
            throw new NotImplementedException();
        }

        public override Guid OpenShop()
        {
            throw new BadStateException($"Tried to invoke OpenShop in Buyer State");
        }

        public override bool AddShopOwner(Guid shopGuid, Guid userGuid)
        {
            throw new BadStateException($"Tried to invoke AddShopOwner in Buyer State");
        }

        public override bool RemoveUser(Guid userToRemoveGuid)
        {
            throw new BadStateException($"Tried to invoke RemoveUser in Buyer State");
        }

        public override bool ConnectToPaymentSystem()
        {
            throw new BadStateException($"Tried to invoke ConnectToPaymentSystem in Buyer State");
        }

        public override bool ConnectToSupplySystem()
        {
            throw new BadStateException($"Tried to invoke ConnectToSupplySystem in Buyer State");
        }

        public override void AddShopProduct(Guid shopGuid, string name, string category, double price, int quantity)
        {
            throw new BadStateException($"Tried to invoke AddShopProduct in Buyer State");
        }

        public override void EditShopProduct(Guid shopGuid, Guid productGuid, double newPrice, int newQuantity)
        {
            throw new BadStateException($"Tried to invoke EditShopProduct in Buyer State");
        }
    }
}
