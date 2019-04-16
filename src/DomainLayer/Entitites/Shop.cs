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
            ShopProducts = new List<ShopProduct>();
            ShopState = ShopStateEnum.Closed;
            UsersPurchaseHistory = new List<Tuple<Guid, ShopProduct>>();
        }

        public void Close()
        {
            VerifyShopIsActive();
            ShopState = ShopStateEnum.Closed;
        }
        public void Adminclose()
        {
            VerifyShopIsActiveOrClosed();
            ShopState = ShopStateEnum.PermanentlyClosed;
        }
        public void Open()
        {
            VerifyShopIsClosed();
            ShopState = ShopStateEnum.Active;
        }

        public Guid AddProduct(Guid userGuid, Product product, double price, int quantity)
        {
            VerifyNotCreatorOrOwnerOrManager(userGuid);

            var newShopProduct = new ShopProduct(product, price, quantity);
            ShopProducts.Add(newShopProduct);
            return newShopProduct.Guid;
        }
        public void EditProduct(Guid userGuid, Guid shopProductGuid, double newPrice, int newQuantity)
        {
            VerifyCreatorOrOwnerOrManager(userGuid);

            var toEdit = ShopProducts.FirstOrDefault(p => p.Guid.Equals(shopProductGuid));
            if (toEdit == null) throw new ProductNotFoundException($"No product with guid - {shopProductGuid} found in shop with guid {Guid}");
            toEdit.Price = newPrice;
            toEdit.Quantity = newQuantity;
        }

        public void RemoveProduct(Guid userGuid, Guid shopProductGuid)
        {
            VerifyCreatorOrOwner(userGuid);

            var toRemove = ShopProducts.FirstOrDefault(p => p.Guid.Equals(shopProductGuid));
            if (toRemove == null) throw new ProductNotFoundException($"No product with guid - {shopProductGuid} found in shop with guid {Guid}");

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
            VerifyCreatorOrOwner(userGuid);
            VerifyNotCreator(ownerToRemoveGuid);
            VerifyOwner(ownerToRemoveGuid);
            VerifyAppointedBy(ownerToRemoveGuid, userGuid);
            foreach(var owner in Owners)
            {
                if (owner.AppointerGuid.Equals(ownerToRemoveGuid)) 
                    CascadeRemoveShopOwner(ownerToRemoveGuid, owner.OwnerGuid); //Cascade
            }
            Owners.Remove(Owners.FirstOrDefault(o => o.OwnerGuid.Equals(ownerToRemoveGuid)));
            return true;
        }

        public void AddShopOwner(Guid userGuid, Guid newOwnerGuid)
        {
            VerifyCreatorOrOwner(userGuid);
            VerifyNotCreatorOrOwnerOrManager(newOwnerGuid);

            var newOwner = new ShopOwner(newOwnerGuid, userGuid, Guid);
            Managers.Add(newOwner);
        }

        public bool RemoveShopManager(Guid userGuid, Guid managerToRemoveGuid)
        {
            VerifyCreatorOrOwner(userGuid);
            VerifyAppointedBy(managerToRemoveGuid, userGuid);
            Managers.Remove(Owners.FirstOrDefault(o => o.OwnerGuid.Equals(managerToRemoveGuid)));
            return true;
        }

        public void AddShopManager(Guid userGuid, Guid newManagaerGuid, List<string> priviliges)
        {
            VerifyCreatorOrOwner(userGuid);
            VerifyNotCreatorOrOwnerOrManager(newManagaerGuid);

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
            if (Creator.Guid.Equals(userGuid))
                return Creator;
            var otherOwner = Owners.FirstOrDefault(o => o.Guid.Equals(userGuid));
            if (otherOwner == null) throw new OwnerNotFoundException($"There is no owner with guid {userGuid} of shop with {Guid}.");
            return otherOwner;
        }
        public bool IsOwner(Guid userGuid)
        {
            if (Creator.Guid.Equals(userGuid))
                return true;
            return Owners.FirstOrDefault(o => o.Guid.Equals(userGuid)) != null;
        }

        public bool AddOwner(ShopOwner oppointer, Guid newOwnerGuid)
        {
            if (IsOwner(newOwnerGuid))
                return false;

            var newOwner = new ShopOwner(newOwnerGuid, oppointer.OwnerGuid, Guid);
            Owners.Add(newOwner);
            return true;
        }

        public bool RemoveOwner(Guid toRemoveOwnerGuid)
        {
            var ownerToRemove = GetOwner(toRemoveOwnerGuid);
            if (ownerToRemove.Guid.Equals(Creator.Guid))
                return false;
            foreach(var otherOwner in Owners)
            {
                if (otherOwner.AppointerGuid.Equals(toRemoveOwnerGuid))
                    RemoveOwner(otherOwner.Guid);
            }
            Owners.Remove(ownerToRemove);// remove the owner from the owners list
            return true;
        }


        public ICollection<Guid> SearchProduct(string productName)
        {
            return ShopProducts
                .Where(sProduct => sProduct.Product.Name.ToLower().Contains(productName.ToLower()))
                .Select(prod => prod.Guid)
                .ToList();
        }

        #region Creator/Owner/Manager Verifiers

        private void VerifyCreatorOrOwner(Guid userGuid)
        {
            if (!IsCreatorOrOwner(userGuid))
            {
                StackTrace stackTrace = new StackTrace();
                throw new NoPriviligesException($"User with guid - {userGuid} Is not a creator " +
                    $"or an owner of the shop with guid - {Guid}, cant complete {stackTrace.GetFrame(1).GetMethod().Name}");
            }
        }

        private void VerifyNotCreator(Guid userGuid)
        {
            if (!userGuid.Equals(Creator.OwnerGuid))
            {
                StackTrace stackTrace = new StackTrace();
                throw new BrokenConstraintException($"User with guid - {userGuid} Is a creator " +
                    $" cant complete {stackTrace.GetFrame(1).GetMethod().Name}");
            }
        }
        private void VerifyNotCreatorOrOwnerOrManager(Guid newManagaerGuid)
        {
            if (IsCreatorOrOwnerOrManager(newManagaerGuid))
            {
                StackTrace stackTrace = new StackTrace();
                throw new BrokenConstraintException($"User with Guid - {newManagaerGuid} is already an owner or a creator or a manager" +
                    $" cant complete {stackTrace.GetFrame(1).GetMethod().Name}");
            }
        }
        private void VerifyCreatorOrOwnerOrManager(Guid newManagaerGuid)
        {
            if (!IsCreatorOrOwnerOrManager(newManagaerGuid))
            {
                StackTrace stackTrace = new StackTrace();
                throw new BrokenConstraintException($"User with Guid - {newManagaerGuid} is not an owner or a creator or a manager" +
                    $" cant complete {stackTrace.GetFrame(1).GetMethod().Name}");
            }
        }
        private void VerifyOwner(Guid ownerToRemoveGuid)
        {
            if (!Owners.Any(owner => owner.OwnerGuid.Equals(ownerToRemoveGuid)))
            {
                StackTrace stackTrace = new StackTrace();
                throw new BrokenConstraintException($"User with Guid - {ownerToRemoveGuid} is not an owner" +
                    $" cant complete {stackTrace.GetFrame(1).GetMethod().Name}");
            }
        }
        private void VerifyAppointedBy(Guid wasAppointed, Guid appointer)
        {
            if (!Owners.FirstOrDefault(o => o.OwnerGuid.Equals(wasAppointed)).AppointerGuid.Equals(appointer))
            {
                StackTrace stackTrace = new StackTrace();
                throw new BrokenConstraintException($"User with Guid - {wasAppointed} was not appointed by {appointer}" +
                    $" cant complete {stackTrace.GetFrame(1).GetMethod().Name}");
            }
        }

        private bool IsCreatorOrOwner(Guid userGuid)
        {
            var isCreator = Creator.OwnerGuid.Equals(userGuid);
            var isOwner = Owners.Any(owner => owner.Guid.Equals(userGuid));
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
        public void VerifyShopProductExists(Guid shopProductGuid)
        {
            var product = ShopProducts.FirstOrDefault(prod => prod.Guid.Equals(shopProductGuid));
            if (product == null)
            {
                StackTrace stackTrace = new StackTrace();
                throw new ProductNotFoundException(string.Format(Resources.EntityNotFound, "product", shopProductGuid) +
                    "In shop with Guid - {Guid}" + $" cant complete {stackTrace.GetFrame(1).GetMethod().Name}");
            }
        }
        #endregion
    }
}
