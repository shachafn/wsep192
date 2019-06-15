using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationCore.Entitites;
using ApplicationCore.Interfaces.DataAccessLayer;

namespace TestsUtils
{
    public class MockBagRepository : IBagRepository
    {
        private ICollection<ShoppingBag> ShoppingBags { get; set; } = new List<ShoppingBag>();

        public void Add(ShoppingBag entity)
        {
            ShoppingBags.Add(entity);
        }

        public void Clear()
        {
            ShoppingBags.Clear();
        }

        public void Delete(ShoppingBag entity)
        {
            ShoppingBags.Remove(entity);
        }

        public ICollection<ShoppingBag> FetchAll()
        {
            return ShoppingBags.ToList();
        }

        /// <summary>
        /// Shouldn't be used by general, since navigation is mostly done through userGuid.
        /// </summary>
        public ShoppingBag FindByIdOrNull(Guid id)
        {
            return ShoppingBags.FirstOrDefault(b => b.Guid.Equals(id));
        }

        public ShoppingBag GetShoppingBagAndCreateIfNeeded(Guid userGuid)
        {
            var bag = ShoppingBags.FirstOrDefault(b => b.UserGuid.Equals(userGuid));
            if (bag == null)
            {
                bag = new ShoppingBag(userGuid);
                ShoppingBags.Add(bag);
            }
            return bag;
        }

        public ShoppingCart GetShoppingCartAndCreateIfNeeded(Guid userGuid, Guid shopGuid)
        {
            var bag = GetShoppingBagAndCreateIfNeeded(userGuid);
            var cart = bag.ShoppingCarts.FirstOrDefault(c => c.ShopGuid.Equals(shopGuid));
            if (cart == null)
            {
                cart = new ShoppingCart(userGuid, shopGuid);
                bag.ShoppingCarts.Add(cart);
            }
            return cart;
        }

        public IQueryable<ShoppingBag> Query()
        {
            return ShoppingBags.AsQueryable();
        }

        public void Update(ShoppingBag entity)
        {
            ShoppingBags.Remove(entity);
            ShoppingBags.Add(entity);
        }
    }
}