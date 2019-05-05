using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLayer.Policies
{
    public interface IDiscountPolicy
    {
        Guid Guid { get; }

        bool CheckPolicy(ref ShoppingCart cart, Guid productGuid, int quantity, BaseUser user);
    }
}
