using System;
using System.Collections.Generic;

namespace ApplicationCore.Entitites
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

        public bool IsEmpty()
        {
            return ShoppingCarts.Count==0;
        }
    }
}
