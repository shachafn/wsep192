﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using Utils;
using ApplicationCore.Interfaces.DAL;

namespace ApplicationCore.Events
{
    public class PurchasedCartEvent : IUpdateEvent
    {
        public Guid ShopGuid { get; set; }
        public Guid Initiator { get; private set; }
        public ICollection<Guid> Targets { get; private set; }
        public string Message { get; private set; }

        public Dictionary<ICollection<Guid>, string> Messages { get; private set; }

        public PurchasedCartEvent(Guid initiator, Guid shopGuid)
        {
            ShopGuid = shopGuid;
            Initiator = initiator;
            Targets = new List<Guid>();
            Message = "UPDATE MESSAGE WAS NOT SET";
            Messages = new Dictionary<ICollection<Guid>, string>();
        }

        public void SetMessage(IUnitOfWork unitOfWork)
        {
            Message = $"{unitOfWork.UserRepository.FindAll().First(u => u.Guid.Equals(Initiator)).Username} bought from your shop {ShopGuid}";
        }

        public void SetTargets(IUnitOfWork unitOfWork)
        {
            var shop = unitOfWork.ShopRepository.FindAll().First(s => s.Guid.Equals(ShopGuid));
            var owners = shop.Owners.Select(owner => owner.OwnerGuid).ToList();
            Targets.Add(shop.Creator.OwnerGuid);
            Targets.AddRange(owners);
        }

        public void SetMessages(IUnitOfWork unitOfWork)
        {
            var shop = unitOfWork.ShopRepository.FindAll().First(s => s.Guid.Equals(ShopGuid));
            var owners = shop.Owners.Select(owner => owner.OwnerGuid).ToList();
            owners.Add(shop.Creator.OwnerGuid);
            owners.Remove(Initiator);
            string ownersMsg = $"{unitOfWork.UserRepository.FindAll().First(u => u.Guid.Equals(Initiator)).Username} bought from your shop {shop.ShopName}";
            string initiatorMsg = $"You bought from shop {shop.ShopName}";
            Messages.Add(owners, ownersMsg);
            Messages.Add(new List<Guid> { Initiator }, initiatorMsg);
        }
    }
}
