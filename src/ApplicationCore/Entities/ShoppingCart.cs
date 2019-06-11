using ApplicationCore.Data;
using ApplicationCore.Entities.Users;
using ApplicationCore.Exceptions;
using DomainLayer.Policies;
using System;
using System.Collections.Generic;
using DomainLayer;

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

        public void PurchaseCart()
        {
            try
            {
                //TODO:Sum the value of the products in the cart and call to external service of payment
                PurchasedProducts = new List<Tuple<ShopProduct, int>>();
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

            //Copy the list so you can iterate and add the discount to it
            ICollection<Tuple<ShopProduct, int>> tempPurchasedProducts = new List<Tuple<ShopProduct, int>>();
            foreach (Tuple<ShopProduct, int> record in cartRef.PurchasedProducts)
            {
                tempPurchasedProducts.Add(record);
            }


            foreach (Tuple<ShopProduct, int> record in tempPurchasedProducts)
            {
                foreach (IDiscountPolicy policy in shop.DiscountPolicies)
                {
                    policy.ApplyPolicy(ref cartRef, record.Item1.Guid, record.Item2, user);
                }
            }

        }
    }
}
