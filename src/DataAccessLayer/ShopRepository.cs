using ApplicationCore.Entitites;
using ApplicationCore.Interfaces.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer
{
    public class ShopRepository : BaseMongoRepository, IShopRepository
    {
        private IContext context;

        public ShopRepository(IContext context) : base(context)
        {
            this.context = context;
        }

        public void Add(Shop entity)
        {
            base.Add<Shop>(entity, context.GetCurrentSession());
        }

        public void Delete(Shop entity)
        {
            base.Delete<Shop>(entity, context.GetCurrentSession());
        }

        public ICollection<Shop> FetchAll()
        {
            return base.FetchAll<Shop>(context.GetCurrentSession());
        }

        public Shop FindByIdOrNull(Guid guid)
        {
            return base.FindByIdOrNull<Shop>(guid, context.GetCurrentSession());
        }

        public IQueryable<Shop> Query()
        {
            return base.Query<Shop>(context.GetCurrentSession());
        }

        public void Update(Shop entity)
        {
            base.Update<Shop>(entity, context.GetCurrentSession());
        }

        public string GetShopName(Guid shopGuid)
        {
            return FindByIdOrNull(shopGuid).ShopName;
        }

        public Shop GetShopByName(string shopName)
        {
            return Query().FirstOrDefault(s => s.ShopName.Equals(shopName));
        }

        public Guid GetShopGuidByName(string shopName)
        {
            return GetShopByName(shopName).Guid;
        }

        public ICollection<Shop> GetActiveShops()
        {
            return Query().Where(s => s.ShopState.Equals(Shop.ShopStateEnum.Active)).ToList();
        }

        public void Clear()
        {
            throw new InvalidOperationException("Clear should only be used for tests.");
        }
    }
}