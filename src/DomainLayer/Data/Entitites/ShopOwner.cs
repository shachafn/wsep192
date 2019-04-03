using System;

namespace DomainLayer.Data.Entitites
{
    public class ShopOwner : BaseEntity // May not need
    {
        public Guid OwnerGuid { get; set; }

        public Guid AppointerGuid { get; set; } // Guid.Empty for the creator of the shop.

        public Shop Shop { get; set; }

        public ShopOwner(Guid ownerGuid, Guid appointerGuid, Shop shop)
        {
            OwnerGuid = ownerGuid;
            AppointerGuid = appointerGuid;
            Shop = shop;
        }

        public ShopOwner(Guid ownerGuid, Shop shop)
        {
            OwnerGuid = ownerGuid;
            AppointerGuid = Guid.Empty;
            Shop = shop;
        }

        public bool AddOwner(Guid userGuid)
        {
            return Shop.AddOwner(this, userGuid);
        }

        public bool RemoveOwner(Guid ownerGuid)
        {
            return Shop.RemoveOwner(this, OwnerGuid);
        }
    }
}
