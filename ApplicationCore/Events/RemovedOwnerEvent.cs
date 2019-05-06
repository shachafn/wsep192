using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.Entitites;

namespace ApplicationCore.Events
{
    public class RemovedOwnerEvent : IUpdateEvent
    {
        public Guid RemovedOwnerGuid { get; private set; }
        public Guid ShopGuid { get; private set; }

        public RemovedOwnerEvent(Guid removedOwnerGuid, Guid shopGuid)
        {
            RemovedOwnerGuid = removedOwnerGuid;
            ShopGuid = shopGuid;
        }
        public ICollection<Guid> GetTargets(ICollection<Shop> shops)
        {
            ICollection<Guid> result = new List<Guid>();

            result.Add(RemovedOwnerGuid);

            return result;
        }

        public string GetMessage()
        {
            return "Hayounous Rules";
        }
    }
}
