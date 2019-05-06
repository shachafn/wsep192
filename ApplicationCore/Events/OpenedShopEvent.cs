using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;

namespace ApplicationCore.Events
{
    public class OpenedShopEvent : IUpdateEvent
    {
        public Guid OpenedShopGuid { get; private set; }
        public Guid ShopGuid { get; private set; }

        public OpenedShopEvent(Guid openedShopGuid, Guid shopGuid)
        {
            OpenedShopGuid = openedShopGuid;
            ShopGuid = shopGuid;
        }

        public string GetMessage()
        {
            return "Shop opened by " + OpenedShopGuid.ToString();
        }

        public ICollection<Guid> GetTargets(ICollection<Shop> shops, ICollection<BaseUser> registeredUsers)
        {
            ICollection<Guid> result = shops.First(s => s.Guid.Equals(ShopGuid)).Owners.Select(owner=>owner.OwnerGuid).ToList();
            result.Add(OpenedShopGuid); // not sure if was added already
            return result;
        }
    }
}
