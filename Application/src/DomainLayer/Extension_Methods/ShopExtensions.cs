using ApplicationCore.Entitites;
using ApplicationCore.Exceptions;
using ApplicationCore.Events;
using ApplicationCore.Data;
using DomainLayer.Policies;
using DomainLayer.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ApplicationCore.Entities.Users;

namespace DomainLayer.Extension_Methods
{
    public static class ShopExtensions
    {
        public static void Close(this Shop shop)
        {
            //throw new NotImplementedException();
            VerifyShopIsActive(shop); ////MOVE TO DOMAINLAYERFACADEVERIFIER WHEN A USE-CASE TO CHANGE SHOP STATUS IS IMPLEMENTED
            shop.ShopState = Shop.ShopStateEnum.Closed;
        }
        public static void ClosePermanently(this Shop shop)
        {
            shop.ShopState = Shop.ShopStateEnum.PermanentlyClosed;
        }
        public static void Reopen(this Shop shop)
        {
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

        public static ICollection<Tuple<Guid, Product, int>> GetPurchaseHistory(this Shop shop, Guid userGuid)
        {
            ICollection<Tuple<Guid, Product, int>> toReturn = new List<Tuple<Guid, Product, int>>();
            foreach (Tuple<Guid, Product, int> purchase in shop.UsersPurchaseHistory)
            {
                if (userGuid.Equals(purchase.Item1))
                    toReturn.Add(new Tuple<Guid, Product, int>(shop.Guid, purchase.Item2, purchase.Item3));
            }
            return toReturn;
        }

        public static bool CascadeRemoveShopOwner(this Shop shop, Guid userGuid, Guid ownerToRemoveGuid)
        {
            foreach (var owner in shop.Owners.ToList())
            {
                if (owner.AppointerGuid.Equals(ownerToRemoveGuid))
                {
                    var newEvent = new RemovedOwnerEvent(owner.OwnerGuid, ownerToRemoveGuid, shop.Guid);
                    newEvent.SetMessages(DomainData.ShopsCollection.Values, DomainData.RegisteredUsersCollection.Values);
                    UpdateCenter.RaiseEvent(newEvent);
                    CascadeRemoveShopOwner(shop, ownerToRemoveGuid, owner.OwnerGuid);
                }
            }
            shop.Owners.Remove(shop.Owners.FirstOrDefault(o => o.OwnerGuid.Equals(ownerToRemoveGuid)));
            return true;
        }

        public static void AddShopOwner(this Shop shop, Guid userGuid, Guid newOwnerGuid)
        {
            var newOwner = new ShopOwner(newOwnerGuid, userGuid, shop.Guid);
            shop.Owners.Add(newOwner);
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

        public static ShopOwner GetOwner(this Shop shop, Guid userGuid)
        {
            if (shop.Creator.OwnerGuid.Equals(userGuid))
                return shop.Creator;
            var otherOwner = shop.Owners.FirstOrDefault(o => o.OwnerGuid.Equals(userGuid));
            if (otherOwner == null) throw new OwnerNotFoundException($"User with Guid - {userGuid} is not an owner of shop {shop.ShopName}.");
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
            int signatures_required = shop.Owners.Count()+1;//+1 for the shop creator
            if (shop.candidate == null && signatures_required > 1) // a new candidate
            {
                var newCandidate = new OwnerCandidate(newOwnerGuid, shop.Guid, appointerGuid, signatures_required);
                shop.candidate = newCandidate;
            }
            else if (shop.candidate != null) // there is a candidate, sign it if possible
            {
                var candidate = shop.candidate;
                if (candidate.signature_target - candidate.Signatures.Count() == 1) // last sign , promote the candidate to a shop owner
                {
                    var newOwner = new ShopOwner(newOwnerGuid, candidate.AppointerGuid, shop.Guid);
                    shop.Owners.Add(newOwner);
                    shop.candidate = null;
                }
                else if (!candidate.Signatures.Values.Contains(appointerGuid)) //if not already signed , sign the candidate
                {
                    candidate.Signatures.Add(DomainData.RegisteredUsersCollection[appointerGuid].Username, appointerGuid);
                }
            }
            else// in the case: adding the second shopowner
            {
                var newOwner = new ShopOwner(newOwnerGuid, appointerGuid, shop.Guid);
                shop.Owners.Add(newOwner);
            }
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
                {
                    shop.RemoveOwner(otherOwner.OwnerGuid);
                    var newEvent = new RemovedOwnerEvent(otherOwner.OwnerGuid, toRemoveOwnerGuid, shop.Guid);
                    newEvent.SetMessages(DomainData.ShopsCollection.Values, DomainData.RegisteredUsersCollection.Values);
                    UpdateCenter.RaiseEvent(newEvent);
                }
            }
            shop.Owners.Remove(ownerToRemove);// remove the owner from the owners list
            return true;
        }

        public static Guid AddNewPurchasePolicy(this Shop shop, IPurchasePolicy newPurchasePolicy)
        {
            if (shop.PurchasePolicies == null)
            {
                shop.PurchasePolicies = new List<IPurchasePolicy>();
            }
            shop.PurchasePolicies.Add(newPurchasePolicy);
            return newPurchasePolicy.Guid;
        }

        public static Guid AddNewDiscountPolicy(this Shop shop, IDiscountPolicy newDiscountPolicy)
        {
            if (shop.DiscountPolicies == null)
            {
                shop.DiscountPolicies = new List<IDiscountPolicy>();
            }
            shop.DiscountPolicies.Add(newDiscountPolicy);
            return newDiscountPolicy.Guid;
        }

        public static bool PurchaseCart(this Shop shop, ShoppingCart cart)
        {
            bool canPurchaseCart = true;

            foreach (var productAndAmountBought in cart.PurchasedProducts)
            {
                var userGuid = cart.UserGuid;
                var actualProduct = shop.ShopProducts.First(p => p.Guid.Equals(productAndAmountBought.Item1));
                BaseUser user = DomainData.RegisteredUsersCollection[userGuid];
                //check purchase policies
                var quantity = productAndAmountBought.Item2;
                foreach(IPurchasePolicy policy in shop.PurchasePolicies)
                    if (!policy.CheckPolicy(cart, productAndAmountBought.Item1, quantity, user))
                        canPurchaseCart = false;
            }
            if (!canPurchaseCart)
                return false;
            
            foreach (var productAndAmountBought in cart.PurchasedProducts)
            {
                var userGuid = cart.UserGuid;
                var actualProduct = shop.ShopProducts.First(p => p.Guid.Equals(productAndAmountBought.Item1));
                //decrease stock quantity
                var quantity = productAndAmountBought.Item2;
                actualProduct.Quantity -= quantity;
                shop.UsersPurchaseHistory.Add(new Tuple<Guid, Product, int>(userGuid, actualProduct.Product, quantity));
            }
            cart.PurchaseCart();
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
                throw new ShopStateException($"Shop {shop.ShopName} is not active. Cant complete method {stackTrace.GetFrame(1).GetMethod().Name}");
            }
        }

        public static void VerifyShopIsClosed(this Shop shop)
        {
            if (!shop.ShopState.Equals(Shop.ShopStateEnum.Closed))
            {
                StackTrace stackTrace = new StackTrace();
                throw new ShopStateException($"Shop {shop.ShopName} is not closed. Cant complete method {stackTrace.GetFrame(1).GetMethod().Name}");
            }
        }

        public static void VerifyShopIsActiveOrClosed(this Shop shop)
        {
            if (!(shop.ShopState.Equals(Shop.ShopStateEnum.Closed) || shop.ShopState.Equals(Shop.ShopStateEnum.Active)))
            {
                StackTrace stackTrace = new StackTrace();
                throw new ShopStateException($"Shop {shop.ShopName} is not closed or active. Cant complete method {stackTrace.GetFrame(1).GetMethod().Name}");
            }
        }

        public static void VerifyShopIsPermanentlyClosed(this Shop shop)
        {
            if (!(shop.ShopState.Equals(Shop.ShopStateEnum.PermanentlyClosed)))
            {
                StackTrace stackTrace = new StackTrace();
                throw new ShopStateException($"Shop {shop.ShopName} is not closed or active. Cant complete method {stackTrace.GetFrame(1).GetMethod().Name}");
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
