using System;
using System.Collections.Generic;
using ApplicationCore.Entitites;

namespace ApplicationCore.Interfaces.DataAccessLayer
{
    public interface IShopRepository : IRepository<Shop>
    {
        string GetShopName(Guid shopGuid);
        Guid GetShopGuidByName(string shopName);
        Shop GetShopByName(string shopName);
        ICollection<Shop> GetActiveShops();
    }
}