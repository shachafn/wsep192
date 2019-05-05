using System;
using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;

namespace DomainLayer.Policies
{
    public interface IPurchasePolicy
    {

        bool CheckPolicy(ShoppingCart cart, Guid productGuid, int quantity, BaseUser user);
        Guid Guid { get; }
    }
}
