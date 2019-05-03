using DomainLayer.Data.Entitites.Users.States;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ApplicationCore.Exceptions;
using DomainLayer.Policies;
using DomainLayer.Data.Entitites.Users;
using ApplicationCore.Entities;

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

        public void AddProductToCart(Guid newShopProductGuid, int amount)
        {
            PurchasedProducts.Add(new Tuple<Guid, int>(newShopProductGuid, amount));
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

        public void PurchaseCart()
        {
            try
            {
                //TODO:Sum the value of the products in the cart and call to external service of payment
            }
            catch
            {
                throw new ExternalServiceFaultException();
            }
        }

        public static void CheckDiscountPolicy(ref ShoppingCart cartRef)
        {
            Shop shop = DomainData.ShopsCollection[cartRef.ShopGuid];
            BaseUser user = DomainData.RegisteredUsersCollection[cartRef.UserGuid];
            foreach (Tuple<Guid,int> record in cartRef.PurchasedProducts)
            {
                foreach(IDiscountPolicy policy in shop.DiscountPolicies)
                {
                    policy.CheckPolicy(ref cartRef, record.Item1,record.Item2, user);
                }
            }

        }
    }
}
