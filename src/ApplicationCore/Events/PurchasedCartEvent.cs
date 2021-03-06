﻿using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationCore.Interfaces.DataAccessLayer;
using Utils;

namespace ApplicationCore.Events
{
    public class PurchasedCartEvent : IUpdateEvent
    {
        public Guid ShopGuid { get; set; }
        public Guid Initiator { get; private set; }
        public ICollection<Guid> Targets { get; private set; }
        public string Message { get; private set; }
        public double TotalPrice { get; private set; }

        public Dictionary<ICollection<Guid>, string> Messages { get; private set; }

        public PurchasedCartEvent(Guid initiator, Guid shopGuid, double totalPrice)
        {
            ShopGuid = shopGuid;
            Initiator = initiator;
            Targets = new List<Guid>();
            Message = "UPDATE MESSAGE WAS NOT SET";
            Messages = new Dictionary<ICollection<Guid>, string>();
            TotalPrice = totalPrice;
        }

        public void SetMessage(IUnitOfWork unitOfWork)
        {
            Message = $"{unitOfWork.BaseUserRepository.GetUsername(Initiator)} bought from your shop {ShopGuid}";
        }

        public void SetTargets(IUnitOfWork unitOfWork)
        {
            var shop = unitOfWork.ShopRepository.FindByIdOrNull(ShopGuid);
            var owners = shop.Owners.Select(owner => owner.OwnerGuid).ToList();
            Targets.Add(shop.Creator.OwnerGuid);
            Targets.AddRange(owners);
        }

        public void SetMessages(IUnitOfWork unitOfWork)
        {
            var shop = unitOfWork.ShopRepository.FindByIdOrNull(ShopGuid);
            var owners = shop.Owners.Select(owner => owner.OwnerGuid).ToList();
            owners.Add(shop.Creator.OwnerGuid);
            owners.Remove(Initiator);
            string ownersMsg;
            if (unitOfWork.BaseUserRepository.Query().Any(u => u.Guid.Equals(Initiator)))
                ownersMsg = $"{unitOfWork.BaseUserRepository.GetUsername(Initiator)} bought from your shop {shop.ShopName}, total price: {TotalPrice}";
            else
                ownersMsg = $"Guest bought from your shop {shop.ShopName}, total price is {TotalPrice}";
            string initiatorMsg = $"You bought from shop {shop.ShopName}, total price is {TotalPrice}";
            Messages.Add(owners, ownersMsg);
            Messages.Add(new List<Guid> { Initiator }, initiatorMsg);
       
        }
    }
}
