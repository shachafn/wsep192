using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using Utils;
using ApplicationCore.Interfaces.DAL;

namespace ApplicationCore.Events
{
    public class OpenedShopEvent : IUpdateEvent
    {
        public Guid ShopGuid { get; private set; }
        public Guid Initiator { get; private set; }
        public ICollection<Guid> Targets { get; private set; }
        public string Message { get; private set; }

        public Dictionary<ICollection<Guid>, string> Messages { get; private set; }

        public OpenedShopEvent(Guid initiator, Guid openedShopGuid)
        {
            ShopGuid = openedShopGuid;
            Initiator = initiator;
            Targets = new List<Guid>();
            Message = "UPDATE MESSAGE WAS NOT SET";
            Messages = new Dictionary<ICollection<Guid>, string>();
        }

        public void SetMessage(IUnitOfWork unitOfWork)
        {
            Message = $"A new shop {ShopGuid} was opened successfully!";
        }

        public void SetMessages(IUnitOfWork unitOfWork)
        {
            string shopName = unitOfWork.ShopRepository.FindAll().FirstOrDefault(shop => shop.Guid.Equals(ShopGuid)).ShopName;
            string initiatorMsg = $"A new shop {shopName} was opened successfully!";
            Messages.Add(new List<Guid> { Initiator }, initiatorMsg);
        }

        public void SetTargets(IUnitOfWork unitOfWork)
        {
            Targets.Add(Initiator);
        }
    }
}
