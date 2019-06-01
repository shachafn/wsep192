﻿using ApplicationCore.Data;
using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using ApplicationCore.Exceptions;
using DomainLayer.Extension_Methods;
using DomainLayer.Policies;
using System;
using System.Collections.Generic;
using System.Linq;
using static ApplicationCore.Entitites.Shop;

namespace DomainLayer.Users.States
{
    public class AdminUserState : AbstractUserState
    {
        public const string AdminUserStateString = "AdminUserState";
        public override ICollection<Guid> GetShoppingHistory()
        {
            throw new BadStateException($"Tried to invoke GetShoppingHistory in Admin State");
        }

        public override Guid OpenShop(BaseUser baseUser)
        {
            throw new BadStateException($"Tried to invoke OpenShop in Admin State");
        }

        public override Guid OpenShop(BaseUser baseUser, string shopName)
        {
            throw new BadStateException($"Tried to invoke OpenShop in Buyer State");
        }

        public override void ReopenShop(Guid shopGuid)
        {
            throw new BadStateException($"Tried to invoke ReopenShop in Admin State");
        }

        public override void CloseShop(Guid shopGuid)
        {
            throw new BadStateException($"Tried to invoke CloseShop in Admin State");
        }

        public override void CloseShopPermanently(Guid shopGuid)
        {
            throw new BadStateException($"Tried to invoke CloseShopPermanently in Admin State");
        }

        public override bool PurchaseCart(BaseUser baseUser, Guid shopGuid)
        {
            throw new BadStateException($"Tried to invoke PurchaseBag in Admin State");
        }

        /// <constraints>
        /// 4. UserToRemove must not be the only owner of an active shop. verfied by facade
        /// 
        /// </constraints>
        public override bool RemoveUser(Guid userToRemoveGuid)
        {

            //if the user is an shop owner\manager Clear shops from the user as creator or other owner
            // and Clear shops from owners or managers appointed by this user 
            ICollection<Shop> shopsOwned = GetShopsOwnedByUser(userToRemoveGuid);
            foreach(Shop shop in shopsOwned)
            {
                shop.RemoveOwner(userToRemoveGuid);
            }
            //Clear user's bag if exsits from the list 
            DomainData.ShoppingBagsCollection.Remove(userToRemoveGuid);
            //Clear shop products--->>???
            //Clear user registration
            DomainData.RegisteredUsersCollection.Remove(userToRemoveGuid);
            //Clear user from logged in - Maybe block this operation if the user is logged in, or its a real pain to cut him off.
            DomainData.LoggedInUsersEntityCollection.Remove(userToRemoveGuid);
            return true;
        }

        private ICollection<Shop> GetShopsOwnedByUser(Guid userToRemoveGuid)
        {
            return DomainData.ShopsCollection.Where
                (shop => shop.ShopState.Equals(ShopStateEnum.Active) &&
                    (shop.Owners.Any(sOwner => sOwner.OwnerGuid.Equals(userToRemoveGuid))
                    ||
                    (shop.Creator.OwnerGuid.Equals(userToRemoveGuid)))).ToList();
        }

        public override bool ConnectToPaymentSystem()
        {
            return true;
            //return External_Services.ExternalServicesManager._paymentSystem.IsAvailable();
        }

        public override bool ConnectToSupplySystem()
        {
            return true;
            //return External_Services.ExternalServicesManager._supplySystem.IsAvailable();
        }

        public override Guid AddProductToShop(BaseUser baseUser, Guid shopGuid, string name, string category, double price, int quantity)
        {
            throw new BadStateException($"Tried to invoke AddProductToShop in Admin State");
        }

        public override void EditProductInShop(BaseUser baseUser, Guid shopGuid, Guid productGuid, double newPrice, int newQuantity)
        {
            throw new BadStateException($"Tried to invoke EditProductInShop in Admin State");
        }

        public override bool RemoveProductFromShop(BaseUser baseUser, Guid shopGuid, Guid shopProductGuid)
        {
            throw new BadStateException($"Tried to invoke RemoveProductFromShop in Admin State");
        }

        public override bool AddProductToCart(BaseUser baseUser, Guid shopGuid, Guid shopProductGuid, int quantity)
        {
            throw new BadStateException($"Tried to invoke AddProductToCart in Admin State");
        }

        public override bool AddShopManager(BaseUser baseUser, Guid shopGuid, Guid newManagaerGuid, List<string> priviliges)
        {
            throw new BadStateException($"Tried to invoke AddShopManager in Admin State");
        }

        public override bool CascadeRemoveShopOwner(BaseUser baseUser, Guid shopGuid, Guid ownerToRemoveGuid)
        {
            throw new BadStateException($"Tried to invoke CascadeRemoveShopOwner in Admin State");
        }

        public override bool EditProductInCart(BaseUser baseUser, Guid shopGuid, Guid shopProductGuid, int newAmount)
        {
            throw new BadStateException($"Tried to invoke EditProductInCart in Admin State");
        }

        public override bool RemoveProductFromCart(BaseUser baseUser, Guid shopGuid, Guid shopProductGuid)
        {
            throw new BadStateException($"Tried to invoke RemoveProductFromCart in Admin State");
        }

        public override ICollection<Guid> GetAllProductsInCart(BaseUser baseUser, Guid shopGuid)
        {
            throw new BadStateException($"Tried to invoke GetAllProductsInCart in Admin State");
        }

        public override bool RemoveShopManager(BaseUser baseUser, Guid shopGuid, Guid managerToRemoveGuid)
        {
            throw new BadStateException($"Tried to invoke RemoveShopManager in Admin State");
        }

        public override bool AddShopOwner(BaseUser baseUser, Guid shopGuid, Guid newManagaerGuid)
        {
            throw new BadStateException($"Tried to invoke AddShopOwner in Admin State");
        }

        public override ICollection<Tuple<ShopProduct, Guid>> SearchProduct(ICollection<string> toMatch, string searchType)
        {
            throw new BadStateException($"Tried to invoke SearchProduct in Admin State");
        }

        public override Guid AddNewPurchasePolicy(Guid userGuid, Guid shopGuid, IPurchasePolicy newPolicy)
        {
            throw new BadStateException($"Tried to invoke AddNewPurchasePolicy in Admin State");
        }

        public override Guid AddNewDiscountPolicy(Guid userGuid, Guid shopGuid, IDiscountPolicy newPolicy)
        {
            throw new BadStateException($"Tried to invoke AddNewDiscountPolicy in Admin State");
        }


    }
}