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


        public override bool AddShopOwner(Guid shopGuid, Guid userGuid)
        {
            throw new BadStateException($"Tried to invoke AddShopOwner in Admin State");
        }

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
            var shopsOwned = GetShopsOwnedByUser(userToRemoveGuid);
            var IsOnlyOwnerByCreator = shopsOwned.Any(shop => shop.Creator.Guid.Equals(userToRemoveGuid) && shop.Owners.Count == 0);
            var IsOnlyOwnerByOtherOwner = shopsOwned.Any(shop => shop.Creator.Guid.Equals(Guid.Empty) && shop.Owners.Count == 1);

            if (IsOnlyOwnerByCreator || IsOnlyOwnerByOtherOwner)
                throw new BrokenConstraintException($"Can't remove user. User with Guid - {Guid}," +
                    $" username - {Username}, is the only owner of an active shop.");

            throw new NotImplementedException();
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

        public override void AddShopProduct(Guid shopGuid, string name, string category, double price, int quantity)
        {
            throw new BadStateException($"Tried to invoke AddProductToShop in Admin State");
        }

        public override void EditShopProduct(Guid shopGuid, Guid productGuid, double newPrice, int newQuantity)
        {
            throw new BadStateException($"Tried to invoke EditShopProduct in Admin State");
        }
    }
}
