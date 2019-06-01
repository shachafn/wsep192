﻿using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using System.Linq;
using Utils;

namespace ApplicationCore.Events
{
    public class ReopenedShopEvent : IUpdateEvent
    {
        public Guid ShopGuid { get; private set; }

        public Guid Initiator { get; private set; }

        public ICollection<Guid> Targets { get; private set; }

        public string Message { get; private set; }

        public Dictionary<ICollection<Guid>, string> Messages { get; }

        public ReopenedShopEvent(Guid initiator, Guid shopGuid)
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
            Message = string.Format("Shop {0} reopened by {1}", ShopGuid, username);
        }
    
        public void SetMessages(ICollection<Shop> shops, ICollection<BaseUser> registeredUsers)
        {
            Shop reopenedShop = shops.FirstOrDefault(shop => shop.Guid.Equals(ShopGuid));
            ICollection<Guid> shopOwners = reopenedShop.Owners.Select(owner => owner.OwnerGuid).ToList();
            shopOwners.Add(reopenedShop.Creator.OwnerGuid);
            shopOwners.Remove(Initiator);
            string username = registeredUsers.FirstOrDefault(user => user.Guid.Equals(Initiator)).Username;
            string ownersMsg = $"Shop {reopenedShop.ShopName} reopened by {username}";
            string initiatorMsg = $"You reopend your shop {reopenedShop.ShopName}";
            Messages.Add(shopOwners, ownersMsg);
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