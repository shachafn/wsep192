using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLayer.Data.Entitites.Users
{
    public interface IUser
    {
        ICollection<ShoppingBag> GetShoppingHistory();

        void PurchaseBag();

        Guid OpenShop();

        bool AddShopOwner(Guid shopGuid, Guid userGuid);

        bool RemoveUser(Guid userToRemoveGuid);

        void AddShopProduct(Guid shopGuid, string name, string category, double price, int quantity);
    }
}
