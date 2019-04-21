using System;
using System.Collections.Generic;
using System.Linq;
using DomainLayer.Exceptions;
using System.Diagnostics;
using DomainLayer.Properties;

namespace DomainLayer.Data.Entitites
{
    public class Shop : BaseEntity
    {
        public ShopOwner Creator { get; }
        public ICollection<ShopOwner> Owners { get; set; }
        public ICollection<ShopOwner> Managers { get; set; }
        public ICollection<ShopProduct> ShopProducts { get; set; }
        public enum ShopStateEnum { Active, Closed, PermanentlyClosed };
        public ShopStateEnum ShopState { get; set; }
        public ICollection<Tuple<Guid,ShopProduct>> UsersPurchaseHistory { get; set; }

        public Shop(Guid ownerGuid)
        {
            Creator = new ShopOwner(ownerGuid, Guid);
            Owners = new List<ShopOwner>();
            Managers = new List<ShopOwner>();
            ShopProducts = new List<ShopProduct>();
            ShopState = ShopStateEnum.Active;
            UsersPurchaseHistory = new List<Tuple<Guid, ShopProduct>>();
        }

        public void Close()
        {
            throw new NotImplementedException();
            VerifyShopIsActive(); ////MOVE TO DOMAINLAYERFACADEVERIFIER WHEN A USE-CASE TO CHANGE SHOP STATUS IS IMPLEMENTED
            ShopState = ShopStateEnum.Closed;
        }
        public void AdminClose()
        {
            throw new NotImplementedException();
            VerifyShopIsActiveOrClosed();////MOVE TO DOMAINLAYERFACADEVERIFIER WHEN A USE-CASE TO CHANGE SHOP STATUS IS IMPLEMENTED
            ShopState = ShopStateEnum.PermanentlyClosed;
        }
        public void Open()
        {
            throw new NotImplementedException();
            VerifyShopIsClosed();////MOVE TO DOMAINLAYERFACADEVERIFIER WHEN A USE-CASE TO CHANGE SHOP STATUS IS IMPLEMENTED
            ShopState = ShopStateEnum.Active;
        }

        public Guid AddProductToShop(Guid userGuid, Product product, double price, int quantity)
        {
            var newShopProduct = new ShopProduct(product, price, quantity);
            ShopProducts.Add(newShopProduct);
            return newShopProduct.Guid;
        }
        public void EditProductInShop(Guid userGuid, Guid shopProductGuid, double newPrice, int newQuantity)
        {
            var toEdit = ShopProducts.FirstOrDefault(p => p.Guid.Equals(shopProductGuid));
            toEdit.Price = newPrice;
            toEdit.Quantity = newQuantity;
        }

        public void RemoveProductFromShop(Guid userGuid, Guid shopProductGuid)
        {
            var toRemove = ShopProducts.FirstOrDefault(p => p.Guid.Equals(shopProductGuid));
            //Need to remove from all users' cart in this shop first, to not break constraint
            ShopProducts.Remove(toRemove);
        }

        public ICollection<ShopProduct> GetPurchaseHistory(Guid userGuid)
        {
            ICollection<ShopProduct> toReturn = new List<ShopProduct>();
            foreach (Tuple<Guid, ShopProduct> purchase in UsersPurchaseHistory)
            {
                if (userGuid.Equals(purchase.Item1))
                    toReturn.Add(purchase.Item2);
            }
            return toReturn;
        }

        public bool CascadeRemoveShopOwner(Guid userGuid, Guid ownerToRemoveGuid)
        {
            foreach(var owner in Owners.ToList())
            {
                if (owner.AppointerGuid.Equals(ownerToRemoveGuid)) 
                    CascadeRemoveShopOwner(ownerToRemoveGuid, owner.OwnerGuid);
            }
            Owners.Remove(Owners.FirstOrDefault(o => o.OwnerGuid.Equals(ownerToRemoveGuid)));
            return true;
        }

        public void AddShopOwner(Guid userGuid, Guid newOwnerGuid)
        {
            var newOwner = new ShopOwner(newOwnerGuid, userGuid, Guid);
            Managers.Add(newOwner);
        }

        public bool RemoveShopManager(Guid userGuid, Guid managerToRemoveGuid)
        {
            Managers.Remove(Owners.FirstOrDefault(o => o.OwnerGuid.Equals(managerToRemoveGuid)));
            return true;
        }

        public void AddShopManager(Guid userGuid, Guid newManagaerGuid, List<string> priviliges)
        {
            var newOwner = new ShopOwner(newManagaerGuid, userGuid, Guid, priviliges);
            Managers.Add(newOwner);
        }

        public void AddToPurchaseHistory(Guid userGuid, ShopProduct shopProduct)
        {
            UsersPurchaseHistory.Add(Tuple.Create(userGuid, shopProduct));
        }


        // Method that overrides the base class (System.Object) implementation.
        public override string ToString()
        {
            return $"Guid - {Guid}, Creator - {Creator}, State - {ShopState}";
        }

        public ShopOwner GetOwner(Guid userGuid)
        {
            if (Creator.OwnerGuid.Equals(userGuid))
                return Creator;
            var otherOwner = Owners.FirstOrDefault(o => o.OwnerGuid.Equals(userGuid));
            if (otherOwner == null) throw new OwnerNotFoundException($"There is no owner with guid {userGuid} of shop with {Guid}.");
            return otherOwner;
        }
        public bool IsOwner(Guid userGuid)
        {
            if (Creator.OwnerGuid.Equals(userGuid))
                return true;
            return Owners.FirstOrDefault(o => o.OwnerGuid.Equals(userGuid)) != null;
        }

        public bool AddOwner(Guid appointerGuid, Guid newOwnerGuid)
        {
            var newOwner = new ShopOwner(newOwnerGuid, appointerGuid, Guid);
            Owners.Add(newOwner);
            return true;
        }

        public bool RemoveOwner(Guid toRemoveOwnerGuid)
        {
            var ownerToRemove = GetOwner(toRemoveOwnerGuid);
            if (ownerToRemove.OwnerGuid.Equals(Creator.OwnerGuid))
                return false;
            foreach(var otherOwner in Owners)
            {
                if (otherOwner.AppointerGuid.Equals(toRemoveOwnerGuid))
                    RemoveOwner(otherOwner.OwnerGuid);
            }
            Owners.Remove(ownerToRemove);// remove the owner from the owners list
            return true;
        }
        #region Creator/Owner/Manager Verifiers

        public void VerifyCreatorOrOwner(Guid userGuid, ICloneableException<Exception> e)
        {
            if (!IsCreatorOrOwner(userGuid))
            {
                StackTrace stackTrace = new StackTrace();
                var msg = $"User with guid - {userGuid} Is not a creator " +
                    $"or an owner of the shop with guid - {Guid}, cant complete {stackTrace.GetFrame(1).GetMethod().Name}";
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
        private bool IsCreatorOrOwnerOrManager(Guid userGuid)
        {
            var isManager = Managers.Any(m => m.OwnerGuid.Equals(userGuid));
            return (IsCreatorOrOwner(userGuid) || isManager);
        }
        #endregion

        #region Status Verifiers
        public void VerifyShopIsActive()
        {
            if (!ShopState.Equals(ShopStateEnum.Active))
            {
                StackTrace stackTrace = new StackTrace();
                throw new ShopStateException($"Shop is not active. Cant complete method {stackTrace.GetFrame(1).GetMethod().Name}");
            }
        }

        public void VerifyShopIsClosed()
        {
            if (!ShopState.Equals(ShopStateEnum.Closed))
            {
                StackTrace stackTrace = new StackTrace();
                throw new ShopStateException($"Shop is not closed. Cant complete method {stackTrace.GetFrame(1).GetMethod().Name}");
            }
        }

        public void VerifyShopIsActiveOrClosed()
        {
            if (!(ShopState.Equals(ShopStateEnum.Closed) && ShopState.Equals(ShopStateEnum.Closed)))
            {
                StackTrace stackTrace = new StackTrace();
                throw new ShopStateException($"Shop is not closed or active. Cant complete method {stackTrace.GetFrame(1).GetMethod().Name}");
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
