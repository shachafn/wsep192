using DomainLayer.Data.Entitites.Users.States;
using System;
using System.Collections.Generic;

namespace DomainLayer.Data.Entitites
{
    public class ShoppingBag : BaseEntity
    {
        public Guid UserGuid { get; set; }

        public ICollection<ShoppingCart> ShoppingCarts { get; set; }

        public ShoppingBag(Guid userGuid)
        {
            UserGuid = userGuid;
            ShoppingCarts = new List<ShoppingCart>();
        }

        
        public void Purchase(AbstractUserState user)
        {
            foreach(ShoppingCart cart in ShoppingCarts)
            {
                cart.PurchaseCart(user);
            }
        }

        public bool Empty()
        {
            return ShoppingCarts.Count==0;
        }
    }
}
