using ApplicationCore.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.Entitites
{
    public class ShoppingCart : BaseEntity
    {
        public Guid UserGuid { get; set; }

        public Guid ShopGuid { get; set; }

        public ICollection<Tuple<ShopProduct, int>> PurchasedProducts { get; set; } // Shop product and quantity that was purchased.

        public ShoppingCart(Guid userGuid, Guid shopGuid)
        {
            UserGuid = userGuid;
            ShopGuid = shopGuid;
            PurchasedProducts = new List<Tuple<ShopProduct, int>>();
        }

        public void AddProductToCart(ShopProduct newShopProduct, int amount)
        {
            PurchasedProducts.Add(new Tuple<ShopProduct, int>(newShopProduct, amount));
        }

        public bool EditProductInCart(Guid shopProductGuid, int newAmount)
        {
            var purchasedProduct = PurchasedProducts.FirstOrDefault(p => p.Item1.Equals(shopProductGuid));
            PurchasedProducts.Remove(purchasedProduct);
            PurchasedProducts.Add(new Tuple<ShopProduct, int>(purchasedProduct.Item1, newAmount));
            //Tuple is immutable so create new one and add it
            return true;
        }

        public bool RemoveProductFromCart(Guid shopProductGuid)
        {
            var purchasedProduct = PurchasedProducts.FirstOrDefault(p => p.Item1.Guid.Equals(shopProductGuid));
            PurchasedProducts.Remove(purchasedProduct);
            return true;
        }
        public ICollection<ShopProduct> GetAllProductsInCart()
        {
            return PurchasedProducts.Select(tuple => tuple.Item1).ToList();
        }

        public void PurchaseCart()
        {
            PurchasedProducts.Clear();
        }
    }
}
