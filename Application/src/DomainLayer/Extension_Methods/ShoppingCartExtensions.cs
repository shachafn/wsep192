using ApplicationCore.Entitites;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DomainLayer.Extension_Methods
{
    public static class ShoppingCartExtensions
    {
        public static void AddProductToCart(this ShoppingCart cart, ShopProduct newShopProduct, int amount)
        {
            cart.PurchasedProducts.Add(new Tuple<ShopProduct, int>(newShopProduct, amount));
        }

        public static bool EditProductInCart(this ShoppingCart cart, Guid shopProductGuid, int newAmount)
        {
            var purchasedProduct = cart.PurchasedProducts.FirstOrDefault(p => p.Item1.Equals(shopProductGuid));
            cart.PurchasedProducts.Remove(purchasedProduct);
            cart.PurchasedProducts.Add(new Tuple<ShopProduct, int>(purchasedProduct.Item1, newAmount));
            //Tuple is immutable so create new one and add it
            return true;
        }

        public static bool RemoveProductFromCart(this ShoppingCart cart, Guid shopProductGuid)
        {
            var purchasedProduct = cart.PurchasedProducts.FirstOrDefault(p => p.Item1.Guid.Equals(shopProductGuid));
            cart.PurchasedProducts.Remove(purchasedProduct);
            return true;
        }
        public static ICollection<ShopProduct> GetAllProductsInCart(this ShoppingCart cart)
        {
            return cart.PurchasedProducts.Select(tuple => tuple.Item1).ToList();
        }

        public static void PurchaseCart(this ShoppingCart cart)
        {
            cart.PurchasedProducts.Clear();
        }

    }
}
