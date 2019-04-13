using DomainLayer.Exceptions;
using System;
using System.Collections.Generic;

namespace DomainLayer.Data.Entitites.Users.States
{
    public class GuestUserState : AbstractUserState
    {

        public override ICollection<ShoppingBag> GetShoppingHistory()
        {
            throw new BadStateException($"Tried to invoke GetShoppingHistory in Guest State");
        }

        public override Guid OpenShop(BaseUser baseUser)
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

        public override Guid AddShopProduct(BaseUser baseUser, Guid shopGuid, string name, string category, double price, int quantity)
        {
            throw new BadStateException($"Tried to invoke AddShopProduct in Guest State");
        }

        public override void EditShopProduct(BaseUser baseUser, Guid shopGuid, Guid productGuid, double newPrice, int newQuantity)
        {
            throw new BadStateException($"Tried to invoke EditShopProduct in Guest State");
        }

        public override bool RemoveShopProduct(BaseUser baseUser, Guid shopGuid, Guid shopProductGuid)
        {
            throw new BadStateException($"Tried to invoke RemoveShopProduct in Guest State");
        }

        public override bool AddProductToShoppingCart(BaseUser baseUser, Guid shopGuid, Guid shopProductGuid, int quantity)
        {
            throw new BadStateException($"Tried to invoke AddProductToShoppingCart in Guest State");
        }

        public override bool AddShopManager(BaseUser baseUser, Guid shopGuid, Guid newManagaerGuid, List<string> priviliges)
        {
            throw new BadStateException($"Tried to invoke AddShopManager in Guest State");
        }

        public override bool CascadeRemoveShopOwner(BaseUser baseUser, Guid shopGuid, Guid ownerToRemoveGuid)
        {
            throw new BadStateException($"Tried to invoke CascadeRemoveShopOwner in Guest State");
        }

        public override bool EditProductInCart(BaseUser baseUser, Guid shopGuid, Guid shopProductGuid, int newAmount)
        {
            throw new BadStateException($"Tried to invoke EditProductInCart in Guest State");
        }

        public override bool RemoveProductFromCart(BaseUser baseUser, Guid shopGuid, Guid shopProductGuid)
        {
            throw new BadStateException($"Tried to invoke RemoveProductFromCart in Guest State");
        }

        public override ICollection<Guid> GetAllProductsInCart(BaseUser baseUser, Guid shopGuid)
        {
            throw new BadStateException($"Tried to invoke GetAllProductsInCart in Guest State");
        }

        public override bool RemoveShopManager(BaseUser baseUser, Guid shopGuid, Guid managerToRemoveGuid)
        {
            throw new BadStateException($"Tried to invoke RemoveShopManager in Guest State");
        }

        public override bool AddShopOwner(BaseUser baseUser, Guid shopGuid, Guid newManagaerGuid)
        {
            throw new BadStateException($"Tried to invoke AddShopOwner in Guest State");
        }
    }
}
