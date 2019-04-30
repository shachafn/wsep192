using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.Entities;
using DomainLayer.Data.Entitites;
using DomainLayer.Data.Entitites.Users;

namespace DomainLayer.Policies
{
    public interface IPurchasePolicy
    {

        bool CheckPolicy(ShoppingCart cart, Guid productGuid, int quantity, BaseUser user);

    }
}
