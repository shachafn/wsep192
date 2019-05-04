using System;
using System.Collections.Generic;

namespace ApplicationCore.Entitites
{
    public class ShopOwner
    {
        public Guid OwnerGuid { get; set; }

        public Guid AppointerGuid { get; set; } // Guid.Empty for the creator of the shop.

        public Guid ShopGuid { get; set; }

        public ICollection<string> Priviliges { get; set; }

        public ShopOwner(Guid ownerGuid, Guid appointerGuid, Guid shopGuid, ICollection<string> priviliges = null)
        {
            OwnerGuid = ownerGuid;
            AppointerGuid = appointerGuid;
            ShopGuid = shopGuid;
            Priviliges = priviliges ?? new List<string>();
        }

        public ShopOwner(Guid ownerGuid, Guid shopGuid, ICollection<string> priviliges = null)
        {
            OwnerGuid = ownerGuid;
            AppointerGuid = Guid.Empty;
            ShopGuid = shopGuid;
            Priviliges = priviliges ?? new List<string>();
        }
    }
}
