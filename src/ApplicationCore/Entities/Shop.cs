using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces.ExternalServices.Properties;
using DomainLayer.Policies;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ApplicationCore.Entitites
{
    public class Shop : BaseEntity
    {
        public ShopOwner Creator { get; set; }
        public ICollection<ShopOwner> Owners { get; set; }
        public ICollection<ShopOwner> Managers { get; set; }
        public ICollection<ShopProduct> ShopProducts { get; set; }
        public enum ShopStateEnum { Active, Closed, PermanentlyClosed };
        public ShopStateEnum ShopState { get; set; }
        public ICollection<Tuple<Guid,ShopProduct,int>> UsersPurchaseHistory { get; set; }
        public ICollection<IPurchasePolicy> PurchasePolicies { get; set; }
        public ICollection<IDiscountPolicy> DiscountPolicies { get; set; }
        public string ShopName { get; set; }
        public OwnerCandidate candidate { get; set; }
        public Shop(Guid ownerGuid)
        {
            Creator = new ShopOwner(ownerGuid, Guid);
            Owners = new List<ShopOwner>();
            Managers = new List<ShopOwner>();
            ShopProducts = new List<ShopProduct>();
            ShopState = ShopStateEnum.Active;
            UsersPurchaseHistory = new List<Tuple<Guid, ShopProduct, int>>();
            PurchasePolicies = new List<IPurchasePolicy>();
            DiscountPolicies = new List<IDiscountPolicy>();
            ShopName = ownerGuid.ToString();
            candidate = null;
        }

        public Shop(Guid ownerGuid, string name) : this (ownerGuid)
        {
            if (name != null && name.Length > 0)
                ShopName = name;
        }

        public ShopOwner GetOwner(Guid ownerGuid) => Owners.FirstOrDefault(o => o.OwnerGuid.Equals(ownerGuid));

        public void Close()
        {
            ShopState = Shop.ShopStateEnum.Closed;
        }
        public void ClosePermanently()
        {
            ShopState = Shop.ShopStateEnum.PermanentlyClosed;
        }
        public void Reopen()
        {
            ShopState = Shop.ShopStateEnum.Active;
        }

        public void AddProductToShop(ShopProduct newShopProduct)
        {
            ShopProducts.Add(newShopProduct);
        }

        public void EditProductInShop(Guid shopProductGuid, double newPrice, int newQuantity)
        {
            var toEdit = ShopProducts.FirstOrDefault(p => p.Guid.Equals(shopProductGuid));
            toEdit.Price = newPrice;
            toEdit.Quantity = newQuantity;
        }

        public void RemoveProductFromShop(Guid shopProductGuid)
        {
            var toRemove = ShopProducts.FirstOrDefault(p => p.Guid.Equals(shopProductGuid));
            //Need to remove from all users' cart in this shop first, to not break constraint
            ShopProducts.Remove(toRemove);
        }

        public ICollection<Tuple<Guid, ShopProduct, int>> GetPurchaseHistory(Guid userGuid)
        {
            ICollection<Tuple<Guid, ShopProduct, int>> toReturn = new List<Tuple<Guid, ShopProduct, int>>();
            foreach (Tuple<Guid, ShopProduct, int> purchase in UsersPurchaseHistory)
            {
                if (userGuid.Equals(purchase.Item1))
                    toReturn.Add(new Tuple<Guid, ShopProduct, int>(Guid, purchase.Item2, purchase.Item3));
            }
            return toReturn;
        }

        public ICollection<Guid> CascadeRemoveShopOwner(Guid userGuid, Guid ownerToRemoveGuid)
        {
            var removedOwners = new HashSet<Guid>();
            removedOwners.Add(ownerToRemoveGuid);
            foreach (var owner in Owners.ToList())
            {
                if (owner.AppointerGuid.Equals(ownerToRemoveGuid))
                {
                    removedOwners.Add(owner.OwnerGuid);
                    var cascadeRemovedOwners = CascadeRemoveShopOwner(ownerToRemoveGuid, owner.OwnerGuid);
                    foreach (var cascRemovedOwner in cascadeRemovedOwners)
                        removedOwners.Add(cascRemovedOwner);
                }
            }
            Owners.Remove(Owners.FirstOrDefault(o => o.OwnerGuid.Equals(ownerToRemoveGuid)));
            return removedOwners;
        }

        public void AddShopOwner(ShopOwner shopOwner)
        {
            Owners.Add(shopOwner);
        }

        public bool RemoveShopManager(Guid userGuid, Guid managerToRemoveGuid)
        {
            return Managers.Remove(Managers.FirstOrDefault(o => o.OwnerGuid.Equals(managerToRemoveGuid)));
        }

        public void AddShopManager(ShopOwner manager)
        {
            Managers.Add(manager);
        }

        public Guid AddNewDiscountPolicy(IDiscountPolicy newDiscountPolicy)
        {
            if (DiscountPolicies == null)
            {
                DiscountPolicies = new List<IDiscountPolicy>();
            }
            DiscountPolicies.Add(newDiscountPolicy);
            return newDiscountPolicy.Guid;
        }

        public Guid AddNewPurchasePolicy(IPurchasePolicy newPurchasePolicy)
        {
            if (PurchasePolicies == null)
            {
                PurchasePolicies = new List<IPurchasePolicy>();
            }
            PurchasePolicies.Add(newPurchasePolicy);
            return newPurchasePolicy.Guid;
        }

        public bool IsOwner(Guid userGuid)
        {
            if (Creator.OwnerGuid.Equals(userGuid))
                return true;
            return Owners.FirstOrDefault(o => o.OwnerGuid.Equals(userGuid)) != null;
        }

        public bool IsManager(Guid userGuid)
        {
            return Managers.Any(manager => manager.OwnerGuid.Equals(userGuid));
        }

        #region Creator/Owner/Manager Verifiers

        private bool IsCreatorOrOwnerOrManager(Guid userGuid)
        {
            var isManager = Managers.Any(m => m.OwnerGuid.Equals(userGuid));
            return (IsCreatorOrOwner(userGuid) || isManager);
        }

        public void VerifyCreatorOrOwner(Guid userGuid, ICloneableException<Exception> e)
        {
            if (!IsCreatorOrOwner(userGuid))
            {
                StackTrace stackTrace = new StackTrace();
                var msg = $"User with guid - {userGuid} Is not a creator " +
                    $"or an owner of the shop with guid - {ShopName}, cant complete {stackTrace.GetFrame(1).GetMethod().Name}";
                throw e.Clone(msg);
            }
        }

        public void VerifyNotCreator(Guid userGuid, ICloneableException<Exception> e)
        {
            if (!userGuid.Equals(Creator.OwnerGuid))
            {
                StackTrace stackTrace = new StackTrace();
                var msg = $"User with guid - {userGuid} Is a creator " +
                    $" cant complete {stackTrace.GetFrame(1).GetMethod().Name}";
                throw e.Clone(msg);
            }
        }
        public void VerifyNotCreatorOrOwnerOrManager(Guid newManagaerGuid, ICloneableException<Exception> e)
        {
            if (IsCreatorOrOwnerOrManager(newManagaerGuid))
            {
                StackTrace stackTrace = new StackTrace();
                var msg = $"User with Guid - {newManagaerGuid} is already an owner or a creator or a manager" +
                    $" cant complete {stackTrace.GetFrame(1).GetMethod().Name}";
                throw e.Clone(msg);
            }
        }
        public void VerifyCreatorOrOwnerOrManager(Guid newManagaerGuid, ICloneableException<Exception> e)
        {
            if (!IsCreatorOrOwnerOrManager(newManagaerGuid))
            {
                StackTrace stackTrace = new StackTrace();
                var msg = $"User with Guid - {newManagaerGuid} is not an owner or a creator or a manager" +
                    $" cant complete {stackTrace.GetFrame(1).GetMethod().Name}";
                throw e.Clone(msg);
            }
        }
        public void VerifyOwnerOrManager(Guid newManagaerGuid, ICloneableException<Exception> e)
        {
            if (!IsOwnerOrManager(newManagaerGuid))
            {
                StackTrace stackTrace = new StackTrace();
                var msg = $"User with Guid - {newManagaerGuid} is not an owner or a creator or a manager" +
                    $" cant complete {stackTrace.GetFrame(1).GetMethod().Name}";
                throw e.Clone(msg);
            }
        }

        public void VerifyOwnerOrCreator(Guid ownerToRemoveGuid, ICloneableException<Exception> e)
        {
            if (!Owners.Any(owner => owner.OwnerGuid.Equals(ownerToRemoveGuid)) && !Creator.OwnerGuid.Equals(ownerToRemoveGuid))
            {
                StackTrace stackTrace = new StackTrace();
                var msg = $"User with Guid - {ownerToRemoveGuid} is not an owner" +
                    $" cant complete {stackTrace.GetFrame(1).GetMethod().Name}";
                throw e.Clone(msg);
            }
        }
        public void VerifyAppointedBy(Guid wasAppointed, Guid appointer, ICloneableException<Exception> e)
        {
            var toRemove = Owners.FirstOrDefault(o => o.OwnerGuid.Equals(wasAppointed));
            if (toRemove == null) toRemove = Managers.FirstOrDefault(m => m.OwnerGuid.Equals(wasAppointed));
            if (toRemove == null)
            {
                StackTrace stackTrace = new StackTrace();
                var msg = $"User with Guid - {wasAppointed} is not an owner or manager of shop {Guid}" +
    $" cant complete {stackTrace.GetFrame(1).GetMethod().Name}";
                throw e.Clone(msg);
            }
            var byCreator = Creator.OwnerGuid.Equals(toRemove.AppointerGuid);
            var byOwner = Owners.Any(o => o.OwnerGuid.Equals(toRemove.AppointerGuid));
            if (!(byCreator || byOwner))
            {
                StackTrace stackTrace = new StackTrace();
                var msg = $"User with Guid - {wasAppointed} was not appointed by {appointer}" +
    $" cant complete {stackTrace.GetFrame(1).GetMethod().Name}";
                throw e.Clone(msg);
            }
        }
        private bool IsOwnerOrManager(Guid userGuid)
        {
            var isOwner = Owners.Any(owner => owner.OwnerGuid.Equals(userGuid));
            var isManager = Managers.Any(manager => manager.OwnerGuid.Equals(userGuid));

            return (isOwner || isManager);
        }
        private bool IsCreatorOrOwner(Guid userGuid)
        {
            var isCreator = Creator.OwnerGuid.Equals(userGuid);
            var isOwner = Owners.Any(owner => owner.OwnerGuid.Equals(userGuid));
            return (isCreator || isOwner);
        }
        #endregion

        #region Status Verifiers
        public void VerifyShopIsActive()
        {
            if (!ShopState.Equals(Shop.ShopStateEnum.Active))
            {
                StackTrace stackTrace = new StackTrace();
                throw new ShopStateException($"Shop {ShopName} is not active. Cant complete method {stackTrace.GetFrame(1).GetMethod().Name}");
            }
        }

        public void VerifyShopIsClosed()
        {
            if (!ShopState.Equals(Shop.ShopStateEnum.Closed))
            {
                StackTrace stackTrace = new StackTrace();
                throw new ShopStateException($"Shop {ShopName} is not closed. Cant complete method {stackTrace.GetFrame(1).GetMethod().Name}");
            }
        }

        public void VerifyShopIsActiveOrClosed()
        {
            if (!(ShopState.Equals(Shop.ShopStateEnum.Closed) || ShopState.Equals(Shop.ShopStateEnum.Active)))
            {
                StackTrace stackTrace = new StackTrace();
                throw new ShopStateException($"Shop {ShopName} is not closed or active. Cant complete method {stackTrace.GetFrame(1).GetMethod().Name}");
            }
        }

        public void VerifyShopIsPermanentlyClosed()
        {
            if (!(ShopState.Equals(Shop.ShopStateEnum.PermanentlyClosed)))
            {
                StackTrace stackTrace = new StackTrace();
                throw new ShopStateException($"Shop {ShopName} is not closed or active. Cant complete method {stackTrace.GetFrame(1).GetMethod().Name}");
            }
        }

        #endregion

        #region Products Verifiers 
        public void VerifyShopProductExists(Guid shopProductGuid, ICloneableException<Exception> e)
        {
            var product = ShopProducts.FirstOrDefault(prod => prod.Guid.Equals(shopProductGuid));
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
