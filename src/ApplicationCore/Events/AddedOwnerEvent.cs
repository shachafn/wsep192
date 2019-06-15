using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationCore.Interfaces.DataAccessLayer;

namespace ApplicationCore.Events
{
    public class AddedOwnerEvent : IUpdateEvent
    {
        public Guid AddedOwnerGuid { get; private set; }
        public Guid ShopGuid { get; private set; }
        public Guid Initiator { get; private set; }
        public ICollection<Guid> Targets { get; private set; }
        public string Message { get; private set; }
        public Dictionary<ICollection<Guid>, string> Messages { get; private set; }

        public AddedOwnerEvent(Guid addedOwnerGuid, Guid initiator, Guid shopGuid)
        {
            AddedOwnerGuid = addedOwnerGuid;
            ShopGuid = shopGuid;
            Initiator = initiator;
            Targets = new List<Guid>();
            Message = "UPDATE MESSAGE WAS NOT SET";
            Messages = new Dictionary<ICollection<Guid>, string>();
        }
        public void SetMessage(IUnitOfWork unitOfWork)
        {
            Message = $"User {unitOfWork.BaseUserRepository.GetUsername(AddedOwnerGuid)} is now an owner of shop {ShopGuid}";
        }

        public void SetTargets(IUnitOfWork unitOfWork)
        {
            Targets.Add(unitOfWork.ShopRepository.FindByIdOrNull(ShopGuid).Creator.OwnerGuid);
            Targets.Add(AddedOwnerGuid);
        }

        public void SetMessages(IUnitOfWork unitOfWork)
        {
            var shop = unitOfWork.ShopRepository.FindByIdOrNull(ShopGuid);
            var otherOwners = shop.Owners.Select(owner => owner.OwnerGuid).ToList();
            otherOwners.Add(shop.Creator.OwnerGuid);
            otherOwners.Remove(Initiator);
            otherOwners.Remove(AddedOwnerGuid);
            string addedOwnerUsername = unitOfWork.BaseUserRepository.GetUsername(AddedOwnerGuid);
            string initiatorUsername = unitOfWork.BaseUserRepository.GetUsername(Initiator);
            string otherOwnersMsg = $"{addedOwnerUsername} is now an owner of shop {shop.ShopName}";
            string initiatorMsg = $"You added {addedOwnerUsername} as a owner of your shop {shop.ShopName}";
            string addedOwnerMsg = $"{initiatorUsername} added you as an owner of shop {shop.ShopName}";
            Messages.Add(new List<Guid> { Initiator }, initiatorMsg);
            Messages.Add(new List<Guid> { AddedOwnerGuid }, addedOwnerMsg);
            Messages.Add(otherOwners, otherOwnersMsg);
        }
    }
}
