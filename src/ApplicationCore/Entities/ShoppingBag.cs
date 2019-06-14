using System;
using System.Collections.Generic;
using System.Linq;

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

        public ShoppingCart GetShoppingCartAndCreateIfNeeded(Guid shopGuid)
        {
            ShoppingCart cart = ShoppingCarts.FirstOrDefault(c => c.ShopGuid.Equals(shopGuid));
            if (!IsCartExists(shopGuid))
            {
                cart = new ShoppingCart(UserGuid, shopGuid);
                ShoppingCarts.Add(cart);
            }
            return cart;
        }

        public bool IsCartExists(Guid shopGuid)
        {
            return ShoppingCarts.Any(c => c.Guid.Equals(shopGuid));
        }
    }
}
