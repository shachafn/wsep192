﻿using System;
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
        public Dictionary<ICollection<Guid>, string> Messages { get; private set; }

        public RemovedOwnerEvent(Guid removedOwnerGuid, Guid initiator, Guid shopGuid)
        {
            RemovedOwnerGuid = removedOwnerGuid;
            ShopGuid = shopGuid;
            Initiator = initiator;
            Targets = new List<Guid>();
            Message = "UPDATE MESSAGE WAS NOT SET";
            Messages = new Dictionary<ICollection<Guid>, string>();
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

        public void SetMessages(ICollection<Shop> shops, ICollection<BaseUser> registeredUsers)
        {
            var shop = shops.First(s => s.Guid.Equals(ShopGuid));
            var otherOwners = shop.Owners.Select(owner => owner.OwnerGuid).ToList();
            otherOwners.Add(shop.Creator.OwnerGuid);
            otherOwners.Remove(Initiator);
            otherOwners.Remove(RemovedOwnerGuid);
            string addedOwnerUsername = registeredUsers.First(u => u.Guid.Equals(RemovedOwnerGuid)).Username;
            string initiatorUsername = registeredUsers.First(u => u.Guid.Equals(Initiator)).Username;
            string otherOwnersMsg = $"{addedOwnerUsername} is no longer an owner of shop {ShopGuid}";
            string initiatorMsg = $"You removed {initiatorUsername} from the owners of your shop {ShopGuid}";
            string removedOwnerMsg = $"{initiatorUsername} removed you from the owners of shop {ShopGuid}";
            Messages.Add(new List<Guid> { Initiator }, initiatorMsg);
            Messages.Add(new List<Guid> { RemovedOwnerGuid }, removedOwnerMsg);
            Messages.Add(otherOwners, otherOwnersMsg);
        }
    }
}
