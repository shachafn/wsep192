using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using Utils;

namespace ApplicationCore.Events
{
    public class AddedManagerEvent : IUpdateEvent
    {
        public Guid AddedManagerGuid { get; private set; }
        public Guid ShopGuid { get; private set; }
        public Guid Initiator { get; private set; }
        public ICollection<Guid> Targets { get; private set; }
        public string Message { get; private set; }
        public Dictionary<ICollection<Guid>, string> Messages { get; private set; }

        public AddedManagerEvent(Guid addedMangerGuid, Guid initiator, Guid shopGuid)
        {
            AddedManagerGuid = addedMangerGuid;
            ShopGuid = shopGuid;
            Initiator = initiator;
            Targets = new List<Guid>();
            Message = "UPDATE MESSAGE WAS NOT SET";
            Messages = new Dictionary<ICollection<Guid>, string>();
        }
        public void SetMessage(ICollection<Shop> shops, ICollection<BaseUser> registeredUsers)
        {
            Message = $"User {registeredUsers.First(u => u.Guid.Equals(AddedManagerGuid)).Username} is now a manager of shop {ShopGuid}";
        }

        public void SetTargets(ICollection<Shop> shops, ICollection<BaseUser> registeredUsers)
        {
            Targets.Add(shops.First(s => s.Guid.Equals(ShopGuid)).Creator.OwnerGuid);
            Targets.Add(AddedManagerGuid);
        }

        public void SetMessages(ICollection<Shop> shops, ICollection<BaseUser> registeredUsers)
        {
            var shop = shops.First(s => s.Guid.Equals(ShopGuid));
            var otherOwners = shop.Owners.Select(owner => owner.OwnerGuid).ToList();
            otherOwners.Add(shop.Creator.OwnerGuid);
            otherOwners.Remove(Initiator);
            otherOwners.Remove(AddedManagerGuid);
            string addedOwnerUsername = registeredUsers.First(u => u.Guid.Equals(AddedManagerGuid)).Username;
            string initiatorUsername = registeredUsers.First(u => u.Guid.Equals(Initiator)).Username;
            string otherOwnersMsg = $"{addedOwnerUsername} is now an manager of shop {shop.ShopName}";
            string initiatorMsg = $"You added {addedOwnerUsername} as a manager of your shop {shop.ShopName}";
            string addedOwnerMsg = $"{initiatorUsername} added you as an manager of shop {shop.ShopName}";
            Messages.Add(new List<Guid> { Initiator }, initiatorMsg);
            Messages.Add(new List<Guid> { AddedManagerGuid }, addedOwnerMsg);
            Messages.Add(otherOwners, otherOwnersMsg);
        }
    }
}
