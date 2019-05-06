using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;

namespace ApplicationCore.Events
{
    public class RemovedOwnerEvent : IUpdateEvent
    {
        public Guid RemovedOwnerGuid { get; private set; }
        public Guid ShopGuid { get; private set; }

        public RemovedOwnerEvent(Guid removedOwnerGuid, Guid shopGuid)
        {
            RemovedOwnerGuid = removedOwnerGuid;
            ShopGuid = shopGuid;
        }
        public ICollection<Guid> GetTargets(ICollection<Shop> shops, ICollection<BaseUser> registeredUsers)
        {
            ICollection<Guid> result = new List<Guid>();

            result.Add(shops.First(s => s.Guid.Equals(ShopGuid)).Creator.OwnerGuid);
            result.Add(RemovedOwnerGuid);

            return result;
        }

        public string GetMessage()
        {
            return "Hayounous Rules";
        }
    }
}
