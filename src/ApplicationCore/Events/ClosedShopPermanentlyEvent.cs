using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationCore.Entitites;
using ApplicationCore.Interfaces.DataAccessLayer;
using Utils;

namespace ApplicationCore.Events
{
    public class ClosedShopPermanentlyEvent : IUpdateEvent
    {
        public Guid ShopGuid { get; private set; }

        public Guid Initiator { get; private set; }

        public ICollection<Guid> Targets { get; private set; }

        public string Message { get; private set; }

        public Dictionary<ICollection<Guid>, string> Messages { get; private set; }

        public ClosedShopPermanentlyEvent(Guid initiator, Guid shopGuid)
        {
            ShopGuid = shopGuid;
            Initiator = initiator;
            Targets = new List<Guid>();
            Message = "UPDATE MESSAGE WAS NOT SET";
            Messages = new Dictionary<ICollection<Guid>, string>();
        }

        public void SetMessage(IUnitOfWork unitOfWork)
        {
            string username = unitOfWork.BaseUserRepository.GetUsername(Initiator);
            Message = string.Format("Shop {0} closed permanently by {1}", ShopGuid, username);
        }

        public void SetTargets(IUnitOfWork unitOfWork)
        {
            Shop reopenedShop = unitOfWork.ShopRepository.FindByIdOrNull(ShopGuid);
            ICollection<Guid> shopOwners = reopenedShop.Owners.Select(owner => owner.OwnerGuid).ToList();
            Targets.Add(reopenedShop.Creator.OwnerGuid);
            Targets.AddRange(shopOwners);
        }

        public void SetMessages(IUnitOfWork unitOfWork)
        {
            Shop closedShop = unitOfWork.ShopRepository.FindByIdOrNull(ShopGuid);
            ICollection<Guid> shopOwners = closedShop.Owners.Select(owner => owner.OwnerGuid).ToList();
            shopOwners.Add(closedShop.Creator.OwnerGuid);
            shopOwners.Remove(Initiator);
            string username = unitOfWork.BaseUserRepository.GetUsername(Initiator);
            string ownersMsg = $"Shop {closedShop.ShopName} closed permanently by {username}";
            string initiatorMsg = $"Shop {closedShop.ShopName} closed permanently by you";
            Messages.Add(shopOwners, ownersMsg);
            Messages.Add(new List<Guid> { Initiator }, initiatorMsg);
        }
    }
}
