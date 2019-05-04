using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities
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
    }
}
