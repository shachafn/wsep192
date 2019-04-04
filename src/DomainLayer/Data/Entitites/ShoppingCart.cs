using DomainLayer.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DomainLayer.Data.Entitites
{
    public class ShoppingCart : BaseEntity
    {
        public Guid UserGuid { get; set; }

        public Guid ShopGuid { get; set; }

        public ICollection<Tuple<Guid,int>> PurchasedProducts { get; set; } // Shop product and quantity that was purchased.

        public ShoppingCart(Guid userGuid, Guid shopGuid)
        {
            UserGuid = userGuid;
            ShopGuid = shopGuid;
            PurchasedProducts = new List<Tuple<Guid, int>>();
        }

        public void AddProductToShoppingCart(Guid newShopProductGuid, int amount)
        {
            throw new NotImplementedException();
        }
        public void RemoveProduct(Guid productGuid)
        {
            throw new NotImplementedException();
        }
        public void EditProduct(Guid productGuid, int newQuantity)
        {
            throw new NotImplementedException();
        }

        public ICollection<ShopProduct> GetAllProducts()
        {
            throw new NotImplementedException();
        }
        public double CalculateTotalPrice()
        {
            throw new NotImplementedException();
        }
        public bool PurchaseCart(User user)
        {
            throw new NotImplementedException();
        }

        public bool EditProductInCart(Guid shopProductGuid, int newAmount)
        {
            var purchasedProduct = PurchasedProducts.FirstOrDefault(p => p.Item1.Equals(shopProductGuid));
            PurchasedProducts.Remove(purchasedProduct);
            PurchasedProducts.Add(new Tuple<Guid, int>(shopProductGuid, newAmount));
            //Tuple is immutable so create new one and add it
            return true;
        }

        public bool RemoveProductFromCart(Guid shopProductGuid)
        {
            var purchasedProduct = PurchasedProducts.FirstOrDefault(p => p.Item1.Equals(shopProductGuid));
            PurchasedProducts.Remove(purchasedProduct);
            return true;
        }
        public ICollection<Guid> GetAllProductsInCart()
        {
            return PurchasedProducts.Select(tuple => tuple.Item1).ToList();
        }

        #region Verifiers
        public void VerifyShopProductDoesNotExist(Guid shopProductGuid)
        {
            var product = PurchasedProducts.FirstOrDefault(prod => prod.Item1.Equals(shopProductGuid));
            if (product != null)
            {
                StackTrace stackTrace = new StackTrace();
                throw new BrokenConstraintException($"Cannot add the same product with Guid - {shopProductGuid} to the cart of user" +
                    $" with Guid - {UserGuid}. Cant complete {stackTrace.GetFrame(1).GetMethod().Name}");
            }
        }

        public void VerifyShopProductExists(Guid shopProductGuid)
        {
            var product = PurchasedProducts.FirstOrDefault(prod => prod.Item1.Equals(shopProductGuid));
            if (product == null)
            {
                StackTrace stackTrace = new StackTrace();
                throw new BrokenConstraintException($"ShopProduct with Guid - {shopProductGuid} diesnt exist in the cart." +
                    $" Cant complete {stackTrace.GetFrame(1).GetMethod().Name}");
            }
        }


        #endregion
    }
}
