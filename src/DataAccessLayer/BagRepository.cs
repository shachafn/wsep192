using ApplicationCore.Interfaces.DataAccessLayer;
using ApplicationCore.Entitites;
using System.Linq;
using System.Collections.Generic;
using System;

namespace DataAccessLayer
{
    internal class BagRepository : BaseMongoRepository, IBagRepository
    {
        private IContext context;

        public BagRepository(IContext context) : base(context)
        {
            this.context = context;
        }


        public void Add(ShoppingBag entity)
        {
            base.Add<ShoppingBag>(entity, context.GetCurrentSession());
        }

        public void Delete(ShoppingBag entity)
        {
            base.Delete<ShoppingBag>(entity, context.GetCurrentSession());
        }

        public ICollection<ShoppingBag> FetchAll()
        {
            return base.FetchAll<ShoppingBag>(context.GetCurrentSession());
        }

        public ShoppingBag FindByIdOrNull(Guid guid)
        {
            return base.FindByIdOrNull<ShoppingBag>(guid, context.GetCurrentSession());
        }

        public IQueryable<ShoppingBag> Query()
        {
            return base.Query<ShoppingBag>(context.GetCurrentSession());
        }

        public void Update(ShoppingBag entity)
        {
            base.Update<ShoppingBag>(entity, context.GetCurrentSession());
        }

        public ShoppingBag GetShoppingBagAndCreateIfNeeded(Guid userGuid)
        {
            var bag = GetByUserGuidOrNulld(userGuid);
            if (bag == null)
            {
                bag = new ShoppingBag(userGuid);
                Add(new ShoppingBag(userGuid));
            }
            Update(bag);
            return bag;
        }

        private ShoppingBag GetByUserGuidOrNulld(Guid userGuid)
        {
            return Query().FirstOrDefault(bag => bag.UserGuid.Equals(userGuid));
        }

        private bool IsBagExists(Guid userGuid)
        {
            return Query().Any(bag => bag.UserGuid.Equals(userGuid));
        }

        public ShoppingCart GetShoppingCartAndCreateIfNeeded(Guid userGuid, Guid shopGuid)
        {
            var bag = GetShoppingBagAndCreateIfNeeded(userGuid);
            var cart = bag.ShoppingCarts.FirstOrDefault(c => c.ShopGuid.Equals(shopGuid));
            if (cart == null)
            {
                cart = new ShoppingCart(bag.UserGuid, shopGuid);
                bag.ShoppingCarts.Add(cart);
            }
            Update(bag);
            return cart;
        }

        public void Clear()
        {
            throw new InvalidOperationException("Clear should only be used for tests.");
        }
    }
}