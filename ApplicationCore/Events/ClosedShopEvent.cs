using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using Utils;

namespace ApplicationCore.Events
{
    public class ClosedShopEvent : IUpdateEvent
    {
        public Guid ShopGuid { get; private set; }

        public Guid Initiator { get; private set; }

        public ICollection<Guid> Targets { get; private set; }

        public string Message { get; private set; }

        public Dictionary<ICollection<Guid>, string> Messages { get; private set; }

        public ClosedShopEvent(Guid initiator, Guid shopGuid)
        {
            ShopGuid = shopGuid;
            Initiator = initiator;
            Targets = new List<Guid>();
            Message = "UPDATE MESSAGE WAS NOT SET";
            Messages = new Dictionary<ICollection<Guid>, string>();
        }

        public void SetMessage(ICollection<Shop> shops, ICollection<BaseUser> registeredUsers)
        {
            string username = registeredUsers.FirstOrDefault(user => user.Guid.Equals(Initiator)).Username;
            Message = string.Format("Shop {0} closed by {1}", ShopGuid, username);
        }

        public void SetMessages(ICollection<Shop> shops, ICollection<BaseUser> registeredUsers)
        {
            Shop closedShop = shops.FirstOrDefault(shop => shop.Guid.Equals(ShopGuid));
            ICollection<Guid> shopOwnersAndAdmins = closedShop.Owners.Select(owner => owner.OwnerGuid).ToList();
            shopOwnersAndAdmins.Add(closedShop.Creator.OwnerGuid);
            shopOwnersAndAdmins.AddRange(registeredUsers.Where(user => user.IsAdmin).Select(user => user.Guid).ToList());
            shopOwnersAndAdmins.Remove(Initiator);
            string username = registeredUsers.FirstOrDefault(user => user.Guid.Equals(Initiator)).Username;
            string ownersAndAdminsMsg = $"Shop {closedShop.ShopName} closed by {username}";
            string initiatorMsg = $"You closed your shop {closedShop.ShopName}";
            Messages.Add(shopOwnersAndAdmins, ownersAndAdminsMsg);
            Messages.Add(new List<Guid> { Initiator }, initiatorMsg);
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
