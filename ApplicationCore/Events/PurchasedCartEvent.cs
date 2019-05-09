using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using Utils;

namespace ApplicationCore.Events
{
    public class PurchasedCartEvent : IUpdateEvent
    {
        public Guid ShopGuid { get; set; }
        public Guid Initiator { get; private set; }
        public ICollection<Guid> Targets { get; private set; }
        public string Message { get; private set; }

        public PurchasedCartEvent(Guid shopGuid, Guid initiator)
        {
            ShopGuid = shopGuid;
            Initiator = initiator;
            Targets = new List<Guid>();
            Message = "UPDATE MESSAGE WAS NOT SET";
        }

        public void SetMessage(ICollection<Shop> shops, ICollection<BaseUser> registeredUsers)
        {
            Message = $"{registeredUsers.First(u => u.Guid.Equals(Initiator)).Username} bought from your shop {ShopGuid}";
        }

        public void SetTargets(ICollection<Shop> shops, ICollection<BaseUser> registeredUsers)
        {
            var shop = shops.First(s => s.Guid.Equals(ShopGuid));
            var owners = shop.Owners.Select(owner => owner.OwnerGuid).ToList();
            Targets.Add(shop.Creator.OwnerGuid);
            Targets.AddRange(owners);
        }
    }
}
