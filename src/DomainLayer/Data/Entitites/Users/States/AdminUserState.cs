using DomainLayer.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using static DomainLayer.Data.Entitites.Shop;

namespace DomainLayer.Data.Entitites.Users.States
{
    public class AdminUserState : AbstractUserState
    {
        public AdminUserState(string username, string password) : base(username, password) { }


        public override ICollection<ShoppingBag> GetShoppingHistory()
        {
            throw new BadStateException($"Tried to invoke GetShoppingHistory in Admin State");
        }

        public override Guid OpenShop()
        {
            throw new BadStateException($"Tried to invoke OpenShop in Admin State");
        }

        public override void PurchaseBag()
        {
            throw new BadStateException($"Tried to invoke PurchaseBag in Admin State");
        }

        /// <constraints>
        /// 4. UserToRemove must not be the only owner of an active shop.
        /// </constraints>
        public override bool RemoveUser(Guid userToRemoveGuid)
        {

            throw new NotImplementedException();
            //Clear shops from owners or managers appointed by this user -- must be checked, might brake constarint every shop must have 1 owner
            //Clear shops from the user as creator or other owner
            //Clear all user's carts in these shops
            //Clear shop products
            //Clear user registration
            //Clear user from logged in - Maybe block this operation if the user is logged in, or its a real pain to cut him off.
        }

        private ICollection<Shop> GetShopsOwnedByUser(Guid userToRemoveGuid)
        {
            return DomainData.ShopsCollection.Where
                (shop => shop.ShopState.Equals(ShopStateEnum.Active) &&
                    (shop.Owners.Any(sOwner => sOwner.OwnerGuid.Equals(userToRemoveGuid))
                    ||
                    (shop.Creator.Guid.Equals(userToRemoveGuid)))).ToList();
        }

        public override bool ConnectToPaymentSystem()
        {
            return External_Services.ExternalServicesManager._paymentSystem.IsAvailable();
        }

        public override bool ConnectToSupplySystem()
        {
            return External_Services.ExternalServicesManager._supplySystem.IsAvailable();
        }

        public override Guid AddShopProduct(Guid shopGuid, string name, string category, double price, int quantity)
        {
            throw new BadStateException($"Tried to invoke AddProductToShop in Admin State");
        }

        public override void EditShopProduct(Guid shopGuid, Guid productGuid, double newPrice, int newQuantity)
        {
            throw new BadStateException($"Tried to invoke EditShopProduct in Admin State");
        }

        public override bool RemoveShopProduct(Guid shopGuid, Guid shopProductGuid)
        {
            throw new BadStateException($"Tried to invoke RemoveShopProduct in Admin State");
        }

        public override bool AddProductToShoppingCart(Guid shopGuid, Guid shopProductGuid, int quantity)
        {
            throw new BadStateException($"Tried to invoke AddProductToShoppingCart in Admin State");
        }

        public override bool AddShopManager(Guid shopGuid, Guid newManagaerGuid, List<string> priviliges)
        {
            throw new BadStateException($"Tried to invoke AddShopManager in Admin State");
        }

        public override bool CascadeRemoveShopOwner(Guid shopGuid, Guid ownerToRemoveGuid)
        {
            throw new BadStateException($"Tried to invoke CascadeRemoveShopOwner in Admin State");
        }

        public override bool EditProductInCart(Guid shopGuid, Guid shopProductGuid, int newAmount)
        {
            throw new BadStateException($"Tried to invoke EditProductInCart in Admin State");
        }

        public override bool RemoveProductFromCart(Guid shopGuid, Guid shopProductGuid)
        {
            throw new BadStateException($"Tried to invoke RemoveProductFromCart in Admin State");
        }

        public override ICollection<Guid> GetAllProductsInCart(Guid shopGuid)
        {
            throw new BadStateException($"Tried to invoke GetAllProductsInCart in Admin State");
        }

        public override bool RemoveShopManager(Guid shopGuid, Guid managerToRemoveGuid)
        {
            throw new BadStateException($"Tried to invoke RemoveShopManager in Admin State");
        }

        public override bool AddShopOwner(Guid shopGuid, Guid newManagaerGuid)
        {
            throw new BadStateException($"Tried to invoke AddShopOwner in Admin State");
        }
    }
}
