using ApplicationCore.Entitites;
using ApplicationCore.Interfaces.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using static ApplicationCore.Entitites.Shop;

namespace TestsUtils
{
    public class MockShopRepository : IShopRepository
    {
        private ICollection<Shop> Shops { get; set; } = new List<Shop>();

        public void Add(Shop entity)
        {
            Shops.Add(entity);
        }

        public void Clear()
        {
            Shops.Clear();
        }

        public void Delete(Shop entity)
        {
            Shops.Remove(entity);
        }

        public ICollection<Shop> FetchAll()
        {
            return Shops.ToList();
        }

        public Shop FindByIdOrNull(Guid id)
        {
            return Shops.FirstOrDefault(b => b.Guid.Equals(id));
        }

        public ICollection<Shop> GetActiveShops()
        {
            return Shops.Where(s => s.ShopState.Equals(ShopStateEnum.Active)).ToList();
        }

        public Shop GetShopByName(string shopName)
        {
            return Shops.FirstOrDefault(s => s.ShopName.Equals(shopName));
        }

        public Guid GetShopGuidByName(string shopName)
        {
            return Shops.FirstOrDefault(s => s.ShopName.Equals(shopName))?.Guid ?? Guid.Empty;
        }

        public string GetShopName(Guid shopGuid)
        {
            return Shops.FirstOrDefault(s => s.Guid.Equals(shopGuid))?.ShopName;
        }

        public IQueryable<Shop> Query()
        {
            return Shops.AsQueryable();
        }

        public void Update(Shop entity)
        {
            Shops.Remove(entity);
            Shops.Add(entity);
        }
    }
}