using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using Utils;

namespace ApplicationCore.Events
{
    public class ClosedShopPermanentlyEvent : IUpdateEvent
    {
        public Guid ShopGuid { get; private set; }

        public Guid Initiator { get; private set; }

        public ICollection<Guid> Targets { get; private set; }

        public string Message { get; private set; }

        public ClosedShopPermanentlyEvent(Guid initiator, Guid shopGuid)
        {
            ShopGuid = shopGuid;
            Initiator = initiator;
            Targets = new List<Guid>();
            Message = "UPDATE MESSAGE WAS NOT SET";
        }

        public void SetMessage(ICollection<Shop> shops, ICollection<BaseUser> registeredUsers)
        {
            string username = registeredUsers.FirstOrDefault(user => user.Guid.Equals(Initiator)).Username;
            Message = string.Format("Shop {0} closed permanently by {1}", ShopGuid, username);
        }

        public void SetTargets(ICollection<Shop> shops, ICollection<BaseUser> registeredUsers)
        {
            Shop reopenedShop = shops.FirstOrDefault(shop => shop.Guid.Equals(ShopGuid));
            ICollection<Guid> shopOwners = reopenedShop.Owners.Select(owner => owner.OwnerGuid).ToList();
            Targets.Add(reopenedShop.Creator.OwnerGuid);
            Targets.AddRange(shopOwners);
        }
    }
}
