using ApplicationCore.Entitites;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DomainLayer.Extension_Methods
{
    public static class ShoppingCartExtensions
    {
        public static void AddProductToCart(this ShoppingCart cart, Guid newShopProductGuid, int amount)
        {
            cart.PurchasedProducts.Add(new Tuple<Guid, int>(newShopProductGuid, amount));
        }

        public static bool EditProductInCart(this ShoppingCart cart, Guid shopProductGuid, int newAmount)
        {
            var purchasedProduct = cart.PurchasedProducts.FirstOrDefault(p => p.Item1.Equals(shopProductGuid));
            cart.PurchasedProducts.Remove(purchasedProduct);
            cart.PurchasedProducts.Add(new Tuple<Guid, int>(shopProductGuid, newAmount));
            //Tuple is immutable so create new one and add it
            return true;
        }

        public static bool RemoveProductFromCart(this ShoppingCart cart, Guid shopProductGuid)
        {
            var purchasedProduct = cart.PurchasedProducts.FirstOrDefault(p => p.Item1.Equals(shopProductGuid));
            cart.PurchasedProducts.Remove(purchasedProduct);
            return true;
        }
        public static ICollection<Guid> GetAllProductsInCart(this ShoppingCart cart)
        {
            return cart.PurchasedProducts.Select(tuple => tuple.Item1).ToList();
        }

        public static void PurchaseCart(this ShoppingCart cart)
        {
            throw new NotImplementedException();
        }
    }
}
