using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;

namespace ApplicationCore.Events
{
    public class PurchasedCartEvent : IUpdateEvent
    {
        public Guid ShopGuid { get; set; }

        public Guid BuyerGuid { get; private set; }
        public string BuyerUsername { get; private set; }

        public PurchasedCartEvent(Guid shopGuid, Guid buyerGuid)
        {
            ShopGuid = shopGuid;
            BuyerGuid = buyerGuid;
        }

        public string GetMessage()
        {
            return string.Format("{0} bought from your shop {1}", BuyerUsername, ShopGuid);
        }

        public ICollection<Guid> GetTargets(ICollection<Shop> shops, ICollection<BaseUser> registeredUsers)
        {
            ICollection<Guid> result = shops.First(shop => shop.Guid.Equals(ShopGuid)).Owners.Select(owner => owner.OwnerGuid).ToList();
            result.Add(shops.First(shop => shop.Guid.Equals(ShopGuid)).Creator.OwnerGuid);
            BuyerUsername = registeredUsers.First(user => user.Guid.Equals(BuyerGuid)).Username;
            return result;
        }
    }
}
