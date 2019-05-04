using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApplicationCore.Data;
using ApplicationCore.Entities;

namespace ApplicationCore.Notifications
{
    static class NotificationsCenter
    {
        public static void NotifyOwnersOfShop(Guid shopGuid, string message)
        {
            Shop shop = DomainData.ShopsCollection[shopGuid];
            foreach (var owner in shop.Owners.ToList())
            {
                NotifyUser(new UserIdentifier(owner.OwnerGuid, false), message);
            }
        }

        public static void NotifyUser(UserIdentifier userIdentifier, string message)
        {
            throw new NotImplementedException();
        }
    }
}
