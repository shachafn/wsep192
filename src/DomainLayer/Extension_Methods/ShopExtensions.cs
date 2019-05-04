﻿using ApplicationCore.Entitites;
using ApplicationCore.Exceptions;
using DomainLayer.Policies;
using DomainLayer.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DomainLayer.Extension_Methods
{
    public static class ShopExtensions
    {
        public static void Close(this Shop shop)
        {
            throw new NotImplementedException();
            VerifyShopIsActive(shop); ////MOVE TO DOMAINLAYERFACADEVERIFIER WHEN A USE-CASE TO CHANGE SHOP STATUS IS IMPLEMENTED
            shop.ShopState = Shop.ShopStateEnum.Closed;
        }
        public static void AdminClose(this Shop shop)
        {
            throw new NotImplementedException();
            VerifyShopIsActiveOrClosed(shop);////MOVE TO DOMAINLAYERFACADEVERIFIER WHEN A USE-CASE TO CHANGE SHOP STATUS IS IMPLEMENTED
            shop.ShopState = Shop.ShopStateEnum.PermanentlyClosed;
        }
        public static void Open(this Shop shop)
        {
            throw new NotImplementedException();
            VerifyShopIsClosed(shop);////MOVE TO DOMAINLAYERFACADEVERIFIER WHEN A USE-CASE TO CHANGE SHOP STATUS IS IMPLEMENTED
            shop.ShopState = Shop.ShopStateEnum.Active;
        }

        public static Guid AddProductToShop(this Shop shop, Guid userGuid, Product product, double price, int quantity)
        {
            var newShopProduct = new ShopProduct(product, price, quantity);
            shop.ShopProducts.Add(newShopProduct);
            return newShopProduct.Guid;
        }
        public static void EditProductInShop(this Shop shop, Guid userGuid, Guid shopProductGuid, double newPrice, int newQuantity)
        {
            var toEdit = shop.ShopProducts.FirstOrDefault(p => p.Guid.Equals(shopProductGuid));
            toEdit.Price = newPrice;
            toEdit.Quantity = newQuantity;
        }

        public static void RemoveProductFromShop(this Shop shop, Guid userGuid, Guid shopProductGuid)
        {
            var toRemove = shop.ShopProducts.FirstOrDefault(p => p.Guid.Equals(shopProductGuid));
            //Need to remove from all users' cart in this shop first, to not break constraint
            shop.ShopProducts.Remove(toRemove);
        }

        public static ICollection<ShopProduct> GetPurchaseHistory(this Shop shop, Guid userGuid)
        {
            ICollection<ShopProduct> toReturn = new List<ShopProduct>();
            foreach (Tuple<Guid, ShopProduct> purchase in shop.UsersPurchaseHistory)
            {
                if (userGuid.Equals(purchase.Item1))
                    toReturn.Add(purchase.Item2);
            }
            return toReturn;
        }

        public static bool CascadeRemoveShopOwner(this Shop shop, Guid userGuid, Guid ownerToRemoveGuid)
        {
            foreach (var owner in shop.Owners.ToList())
            {
                if (owner.AppointerGuid.Equals(ownerToRemoveGuid))
                    CascadeRemoveShopOwner(shop, ownerToRemoveGuid, owner.OwnerGuid);
            }
            shop.Owners.Remove(shop.Owners.FirstOrDefault(o => o.OwnerGuid.Equals(ownerToRemoveGuid)));
            return true;
        }

        public static void AddShopOwner(this Shop shop, Guid userGuid, Guid newOwnerGuid)
        {
            var newOwner = new ShopOwner(newOwnerGuid, userGuid, shop.Guid);
            shop.Managers.Add(newOwner);
        }

        public static bool RemoveShopManager(this Shop shop, Guid userGuid, Guid managerToRemoveGuid)
        {
            shop.Managers.Remove(shop.Owners.FirstOrDefault(o => o.OwnerGuid.Equals(managerToRemoveGuid)));
            return true;
        }

        public static void AddShopManager(this Shop shop, Guid userGuid, Guid newManagaerGuid, List<string> priviliges)
        {
            var newOwner = new ShopOwner(newManagaerGuid, userGuid, shop.Guid, priviliges);
            shop.Managers.Add(newOwner);
        }

        public static void AddToPurchaseHistory(this Shop shop, Guid userGuid, ShopProduct shopProduct)
        {
            shop.UsersPurchaseHistory.Add(Tuple.Create(userGuid, shopProduct));
        }

        public static ShopOwner GetOwner(this Shop shop, Guid userGuid)
        {
            if (shop.Creator.OwnerGuid.Equals(userGuid))
                return shop.Creator;
            var otherOwner = shop.Owners.FirstOrDefault(o => o.OwnerGuid.Equals(userGuid));
            if (otherOwner == null) throw new OwnerNotFoundException($"There is no owner with guid {userGuid} of shop with {shop.Guid}.");
            return otherOwner;
        }
        public static bool IsOwner(this Shop shop, Guid userGuid)
        {
            if (shop.Creator.OwnerGuid.Equals(userGuid))
                return true;
            return shop.Owners.FirstOrDefault(o => o.OwnerGuid.Equals(userGuid)) != null;
        }

        public static bool AddOwner(this Shop shop, Guid appointerGuid, Guid newOwnerGuid)
        {
            var newOwner = new ShopOwner(newOwnerGuid, appointerGuid, shop.Guid);
            shop.Owners.Add(newOwner);
            return true;
        }

        public static bool RemoveOwner(this Shop shop, Guid toRemoveOwnerGuid)
        {
            var ownerToRemove = shop.GetOwner(toRemoveOwnerGuid);
            if (ownerToRemove.OwnerGuid.Equals(shop.Creator.OwnerGuid))
                return false;
            foreach (var otherOwner in shop.Owners)
            {
                if (otherOwner.AppointerGuid.Equals(toRemoveOwnerGuid))
                    shop.RemoveOwner(otherOwner.OwnerGuid);
            }
            shop.Owners.Remove(ownerToRemove);// remove the owner from the owners list
            return true;
        }

        public static bool AddNewPurchasePolicy(this Shop shop,IPurchasePolicy newPurchasePolicy)
        {
            shop.PurchasePolicies.Add(newPurchasePolicy);
            return true;
        }

        public static bool AddNewDiscountPolicy(this Shop shop, IDiscountPolicy newDiscountPolicy)
        {
            shop.DiscountPolicies.Add(newDiscountPolicy);
            return true;
        }
        #region Creator/Owner/Manager Verifiers

        public static void VerifyCreatorOrOwner(this Shop shop, Guid userGuid, ICloneableException<Exception> e)
        {
            if (!shop.IsCreatorOrOwner(userGuid))
            {
                StackTrace stackTrace = new StackTrace();
                var msg = $"User with guid - {userGuid} Is not a creator " +
                    $"or an owner of the shop with guid - {shop.Guid}, cant complete {stackTrace.GetFrame(1).GetMethod().Name}";
                throw e.Clone(msg);
            }
        }

        public static void VerifyNotCreator(this Shop shop, Guid userGuid, ICloneableException<Exception> e)
        {
            if (!userGuid.Equals(shop.Creator.OwnerGuid))
            {
                StackTrace stackTrace = new StackTrace();
                var msg = $"User with guid - {userGuid} Is a creator " +
                    $" cant complete {stackTrace.GetFrame(1).GetMethod().Name}";
                throw e.Clone(msg);
            }
        }
        public static void VerifyNotCreatorOrOwnerOrManager(this Shop shop, Guid newManagaerGuid, ICloneableException<Exception> e)
        {
            if (shop.IsCreatorOrOwnerOrManager(newManagaerGuid))
            {
                StackTrace stackTrace = new StackTrace();
                var msg = $"User with Guid - {newManagaerGuid} is already an owner or a creator or a manager" +
                    $" cant complete {stackTrace.GetFrame(1).GetMethod().Name}";
                throw e.Clone(msg);
            }
        }
        public static void VerifyCreatorOrOwnerOrManager(this Shop shop, Guid newManagaerGuid, ICloneableException<Exception> e)
        {
            if (!shop.IsCreatorOrOwnerOrManager(newManagaerGuid))
            {
                StackTrace stackTrace = new StackTrace();
                var msg = $"User with Guid - {newManagaerGuid} is not an owner or a creator or a manager" +
                    $" cant complete {stackTrace.GetFrame(1).GetMethod().Name}";
                throw e.Clone(msg);
            }
        }
        public static void VerifyOwnerOrManager(this Shop shop, Guid newManagaerGuid, ICloneableException<Exception> e)
        {
            if (!shop.IsOwnerOrManager(newManagaerGuid))
            {
                StackTrace stackTrace = new StackTrace();
                var msg = $"User with Guid - {newManagaerGuid} is not an owner or a creator or a manager" +
                    $" cant complete {stackTrace.GetFrame(1).GetMethod().Name}";
                throw e.Clone(msg);
            }
        }

        public static void VerifyOwnerOrCreator(this Shop shop, Guid ownerToRemoveGuid, ICloneableException<Exception> e)
        {
            if (!shop.Owners.Any(owner => owner.OwnerGuid.Equals(ownerToRemoveGuid)) && !shop.Creator.OwnerGuid.Equals(ownerToRemoveGuid))
            {
                StackTrace stackTrace = new StackTrace();
                var msg = $"User with Guid - {ownerToRemoveGuid} is not an owner" +
                    $" cant complete {stackTrace.GetFrame(1).GetMethod().Name}";
                throw e.Clone(msg);
            }
        }
        public static void VerifyAppointedBy(this Shop shop, Guid wasAppointed, Guid appointer, ICloneableException<Exception> e)
        {
            var toRemove = shop.Owners.FirstOrDefault(o => o.OwnerGuid.Equals(wasAppointed));
            if (toRemove == null) toRemove = shop.Managers.FirstOrDefault(m => m.OwnerGuid.Equals(wasAppointed));
            if (toRemove == null)
            {
                StackTrace stackTrace = new StackTrace();
                var msg = $"User with Guid - {wasAppointed} is not an owner or manager of shop {shop.Guid}" +
    $" cant complete {stackTrace.GetFrame(1).GetMethod().Name}";
                throw e.Clone(msg);
            }
            var byCreator = shop.Creator.OwnerGuid.Equals(toRemove.AppointerGuid);
            var byOwner = shop.Owners.Any(o => o.OwnerGuid.Equals(toRemove.AppointerGuid));
            if (!(byCreator || byOwner))
            {
                StackTrace stackTrace = new StackTrace();
                var msg = $"User with Guid - {wasAppointed} was not appointed by {appointer}" +
    $" cant complete {stackTrace.GetFrame(1).GetMethod().Name}";
                throw e.Clone(msg);
            }
        }
        private static bool IsOwnerOrManager(this Shop shop, Guid userGuid)
        {
            var isOwner = shop.Owners.Any(owner => owner.OwnerGuid.Equals(userGuid));
            var isManager = shop.Managers.Any(manager => manager.OwnerGuid.Equals(userGuid));

            return (isOwner || isManager);
        }
        private static bool IsCreatorOrOwner(this Shop shop, Guid userGuid)
        {
            var isCreator = shop.Creator.OwnerGuid.Equals(userGuid);
            var isOwner = shop.Owners.Any(owner => owner.OwnerGuid.Equals(userGuid));
            return (isCreator || isOwner);
        }
        private static bool IsCreatorOrOwnerOrManager(this Shop shop, Guid userGuid)
        {
            var isManager = shop.Managers.Any(m => m.OwnerGuid.Equals(userGuid));
            return (shop.IsCreatorOrOwner(userGuid) || isManager);
        }
        #endregion

        #region Status Verifiers
        public static void VerifyShopIsActive(this Shop shop)
        {
            if (!shop.ShopState.Equals(Shop.ShopStateEnum.Active))
            {
                StackTrace stackTrace = new StackTrace();
                throw new ShopStateException($"Shop is not active. Cant complete method {stackTrace.GetFrame(1).GetMethod().Name}");
            }
        }

        public static void VerifyShopIsClosed(this Shop shop)
        {
            if (!shop.ShopState.Equals(Shop.ShopStateEnum.Closed))
            {
                StackTrace stackTrace = new StackTrace();
                throw new ShopStateException($"Shop is not closed. Cant complete method {stackTrace.GetFrame(1).GetMethod().Name}");
            }
        }

        public static void VerifyShopIsActiveOrClosed(this Shop shop)
        {
            if (!(shop.ShopState.Equals(Shop.ShopStateEnum.Closed) && shop.ShopState.Equals(Shop.ShopStateEnum.Closed)))
            {
                StackTrace stackTrace = new StackTrace();
                throw new ShopStateException($"Shop is not closed or active. Cant complete method {stackTrace.GetFrame(1).GetMethod().Name}");
            }
        }

        #endregion

        #region Products Verifiers 
        public static void VerifyShopProductExists(this Shop shop, Guid shopProductGuid, ICloneableException<Exception> e)
        {
            var product = shop.ShopProducts.FirstOrDefault(prod => prod.Guid.Equals(shopProductGuid));
            if (product == null)
            {
                StackTrace stackTrace = new StackTrace();
                var msg = string.Format(Resources.EntityNotFound, "product", shopProductGuid) +
                    "In shop with Guid - {Guid}" + $" cant complete {stackTrace.GetFrame(1).GetMethod().Name}";
                throw e.Clone(msg);
            }
        }
        #endregion
    }
}
