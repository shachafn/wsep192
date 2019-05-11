using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using Utils;

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

        public void SetMessage(ICollection<Shop> shops, ICollection<BaseUser> registeredUsers)
        {
            Message = $"A new shop {ShopGuid} was opened successfully!";
        }

        public void SetMessages(ICollection<Shop> shops, ICollection<BaseUser> registeredUsers)
        {
            string initiatorMsg = $"A new shop {ShopGuid} was opened successfully!";
            Messages.Add(new List<Guid> { Initiator }, initiatorMsg);
        }

        public void SetTargets(ICollection<Shop> shops, ICollection<BaseUser> registeredUsers)
        {
            Targets.Add(Initiator);
        }
    }
}
