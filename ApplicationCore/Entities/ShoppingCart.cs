using ApplicationCore.Data;
using ApplicationCore.Entities.Users;
using ApplicationCore.Exceptions;
using DomainLayer.Policies;
using System;
using System.Collections.Generic;

namespace ApplicationCore.Entitites
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

        public void PurchaseCart()
        {
            try
            {
                //TODO:Sum the value of the products in the cart and call to external service of payment
                PurchasedProducts = new List<Tuple<Guid, int>>();
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
            foreach (Tuple<Guid, int> record in cartRef.PurchasedProducts)
            {
                foreach (IDiscountPolicy policy in shop.DiscountPolicies)
                {
                    policy.ApplyPolicy(ref cartRef, record.Item1, record.Item2, user);
                }
            }

        }
    }
}
