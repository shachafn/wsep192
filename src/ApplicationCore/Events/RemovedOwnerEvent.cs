using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationCore.Interfaces.DataAccessLayer;

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
        public void SetMessage(IUnitOfWork unitOfWork)
        {
            Message = $"User {unitOfWork.BaseUserRepository.GetUsername(RemovedOwnerGuid)} is no longer an owner of shop {ShopGuid}";
        }
        public void SetTargets(IUnitOfWork unitOfWork)
        {
            Targets.Add(unitOfWork.ShopRepository.FindByIdOrNull(ShopGuid).Creator.OwnerGuid);
            Targets.Add(RemovedOwnerGuid);
        }

        public void SetMessages(IUnitOfWork unitOfWork)
        {
            var shop = unitOfWork.ShopRepository.FindByIdOrNull(ShopGuid);
            var otherOwners = shop.Owners.Select(owner => owner.OwnerGuid).ToList();
            otherOwners.Add(shop.Creator.OwnerGuid);
            otherOwners.Remove(Initiator);
            otherOwners.Remove(RemovedOwnerGuid);
            string removedOwnerUsername = unitOfWork.BaseUserRepository.GetUsername(RemovedOwnerGuid);
            string initiatorUsername = unitOfWork.BaseUserRepository.GetUsername(Initiator);
            string otherOwnersMsg = $"{removedOwnerUsername} is no longer an owner of shop {shop.ShopName}";
            string initiatorMsg = $"You removed {removedOwnerUsername} from the owners of your shop {shop.ShopName}";
            string removedOwnerMsg = $"{initiatorUsername} removed you from the owners of shop {shop.ShopName}";
            Messages.Add(new List<Guid> { Initiator }, initiatorMsg);
            Messages.Add(new List<Guid> { RemovedOwnerGuid }, removedOwnerMsg);
            Messages.Add(otherOwners, otherOwnersMsg);
        }
    }
}
