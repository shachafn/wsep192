using System;
using System.Collections.Generic;
using System.Linq;
using DomainLayer.Exceptions;

namespace DomainLayer.Data.Entitites
{
    public class Shop : BaseEntity
    {
        public ShopOwner Creator { get; }

        public ICollection<ShopOwner> Owners { get; set; }

        public ICollection<ShopProduct> ShopProducts { get; set; }

        public enum ShopStateEnum { Active, Closed, PermanentlyClosed };
        public ShopStateEnum ShopState { get; set; }

        public ICollection<Tuple<Guid,ShopProduct>> UsersPurchaseHistory { get; set; }

        public Shop(Guid ownerGuid)
        {
            Creator = new ShopOwner(ownerGuid, this);
            Owners = new List<ShopOwner>();
            ShopProducts = new List<ShopProduct>();
            ShopState = ShopStateEnum.Closed;
            UsersPurchaseHistory = new List<Tuple<Guid, ShopProduct>>();
        }

        public void Close()
        {
            if (ShopState.Equals(ShopStateEnum.Active))
                ShopState = ShopStateEnum.Closed;
        }
        public void Adminclose()
        {
            if (ShopState.Equals(ShopStateEnum.Active) || ShopState.Equals(ShopStateEnum.Closed))
                ShopState = ShopStateEnum.PermanentlyClosed;
        }
        public void Open()
        {
            if (ShopState.Equals(ShopStateEnum.Closed))
            {
                ShopState = ShopStateEnum.Active;
            }
        }

        public void AddProduct(Guid userGuid, Product product, double price, int quantity)
        {
            if (!(Creator.OwnerGuid.Equals(userGuid) || Owners.Any(o => o.Guid.Equals(userGuid))))
                throw new NoPriviligesException($"User with guid - {userGuid} Is not a creator " +
                    $"or an owner of the shop with guid - {Guid}, cant add a new product.");

            ShopProduct newShopProduct = new ShopProduct(product, price, quantity);
            ShopProducts.Add(newShopProduct);
        }
        public void RemoveProduct(Guid productGuid)
        {
            var toRemove = ShopProducts.FirstOrDefault(sProduct => sProduct.Guid.Equals(productGuid));
            if (toRemove != null)
                ShopProducts.Remove(toRemove);
        }

        public ShopProduct SearchProduct(Guid productGuid)
        {
            return ShopProducts.FirstOrDefault(sProduct => sProduct.Product.Guid.Equals(productGuid));            
        }
        public void EditProduct(Guid userGuid, Guid productGuid, double newPrice, int newQuantity)
        {
            if (!(Creator.OwnerGuid.Equals(userGuid) || Owners.Any(o => o.Guid.Equals(userGuid))))
                throw new NoPriviligesException($"User with guid - {userGuid} Is not a creator " +
                    $"or an owner of the shop with guid - {Guid}, cant add a new product.");

            var toEdit = ShopProducts.FirstOrDefault(p => p.Guid.Equals(productGuid));
            if (toEdit == null) throw new ProductNotFoundException($"No product with guid - {productGuid} found in shop with guid {Guid}");
            toEdit.Price = newPrice;
            toEdit.Quantity = newQuantity;
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

        public void AddToPurchaseHistory(Guid userGuid, ShopProduct shopProduct)
        {
            UsersPurchaseHistory.Add(Tuple.Create(userGuid, shopProduct));
        }
        public IEnumerable<Product> SearchProducts(string searchString)
        {
            List<Product> toReturn = new List<Product>();
            foreach (ShopProduct sp in ShopProducts)
            {
                if (sp.Product.Name.Contains(searchString) || sp.Product.Category.Contains(searchString))
                    toReturn.Add(sp.Product);
            }
            return toReturn;
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

            var newOwner = new ShopOwner(newOwnerGuid, oppointer.OwnerGuid, this);
            Owners.Add(newOwner);
            return true;
        }

        public bool RemoveOwner(ShopOwner oppointer, Guid toRemoveOwnerGuid)
        {
            var ownerToRemove = GetOwner(toRemoveOwnerGuid);
            if (ownerToRemove.Guid.Equals(Creator.Guid))
                return false;
            foreach(var otherOwner in Owners)
            {
                if (otherOwner.AppointerGuid.Equals(toRemoveOwnerGuid))
                    RemoveOwner(ownerToRemove, otherOwner.Guid);
            }
            return true;
        }
    }
}
