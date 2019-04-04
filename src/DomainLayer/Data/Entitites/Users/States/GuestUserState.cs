using DomainLayer.Exceptions;
using System;
using System.Collections.Generic;

namespace DomainLayer.Data.Entitites.Users.States
{
    public class GuestUserState : AbstractUserState
    {
        public GuestUserState(string username, string password) : base(username, password) { }

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

        public override Guid AddShopProduct(Guid shopGuid, string name, string category, double price, int quantity)
        {
            throw new BadStateException($"Tried to invoke AddShopProduct in Guest State");
        }

        public override void EditShopProduct(Guid shopGuid, Guid productGuid, double newPrice, int newQuantity)
        {
            throw new BadStateException($"Tried to invoke EditShopProduct in Guest State");
        }

        public override bool RemoveShopProduct(Guid shopGuid, Guid shopProductGuid)
        {
            throw new BadStateException($"Tried to invoke RemoveShopProduct in Guest State");
        }

        public override bool AddProductToShoppingCart(Guid shopGuid, Guid shopProductGuid, int quantity)
        {
            throw new BadStateException($"Tried to invoke AddProductToShoppingCart in Guest State");
        }

        public override bool AddShopManager(Guid shopGuid, Guid newManagaerGuid, List<string> priviliges)
        {
            throw new BadStateException($"Tried to invoke AddShopManager in Guest State");
        }

        public override bool CascadeRemoveShopOwner(Guid shopGuid, Guid ownerToRemoveGuid)
        {
            throw new BadStateException($"Tried to invoke CascadeRemoveShopOwner in Guest State");
        }

        public override bool EditProductInCart(Guid shopGuid, Guid shopProductGuid, int newAmount)
        {
            throw new BadStateException($"Tried to invoke EditProductInCart in Guest State");
        }

        public override bool RemoveProductFromCart(Guid shopGuid, Guid shopProductGuid)
        {
            throw new BadStateException($"Tried to invoke RemoveProductFromCart in Guest State");
        }

        public override ICollection<Guid> GetAllProductsInCart(Guid shopGuid)
        {
            throw new BadStateException($"Tried to invoke GetAllProductsInCart in Guest State");
        }

        public override bool RemoveShopManager(Guid shopGuid, Guid managerToRemoveGuid)
        {
            throw new BadStateException($"Tried to invoke RemoveShopManager in Guest State");
        }

        public override bool AddShopOwner(Guid shopGuid, Guid newManagaerGuid)
        {
            throw new BadStateException($"Tried to invoke AddShopOwner in Guest State");
        }
    }
}
