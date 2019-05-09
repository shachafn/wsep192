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
        public Guid Initiator { get; private set; }
        public ICollection<Guid> Targets { get; private set; }
        public string Message { get; private set; }

        public RemovedOwnerEvent(Guid removedOwnerGuid, Guid shopGuid, Guid initiator)
        {
            RemovedOwnerGuid = removedOwnerGuid;
            ShopGuid = shopGuid;
            Initiator = initiator;
            Targets = new List<Guid>();
            Message = "UPDATE MESSAGE WAS NOT SET";
        }
        public void SetMessage(ICollection<Shop> shops, ICollection<BaseUser> registeredUsers)
        {
            Message = $"User {registeredUsers.First(u => u.Guid.Equals(RemovedOwnerGuid)).Username} is no longer an owner of shop {ShopGuid}";
        }
        public void SetTargets(ICollection<Shop> shops, ICollection<BaseUser> registeredUsers)
        {
            Targets.Add(shops.First(s => s.Guid.Equals(ShopGuid)).Creator.OwnerGuid);
            Targets.Add(RemovedOwnerGuid);
        }
    }
}
