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
        public Guid OpenedUserGuid { get; private set; }
        public string OpenedUserName { get; private set; }
        public Guid ShopGuid { get; private set; }

        public OpenedShopEvent(Guid openedShopGuid, Guid shopGuid)
        {
            OpenedUserGuid = openedShopGuid;
            ShopGuid = shopGuid;
        }

        public string GetMessage()
        {
            return string.Format("Shop {0} opened by {1}", ShopGuid, OpenedUserName);
        }

        public ICollection<Guid> GetTargets(ICollection<Shop> shops, ICollection<BaseUser> registeredUsers)
        {
            ICollection<Guid> result = shops.First(s => s.Guid.Equals(ShopGuid)).Owners.Select(owner => owner.OwnerGuid).ToList();
            result.Add(OpenedUserGuid); // not sure if was added already
            OpenedUserName = registeredUsers.First(user => user.Guid.Equals(OpenedUserGuid)).Username;
            return result;
        }
    }
}
