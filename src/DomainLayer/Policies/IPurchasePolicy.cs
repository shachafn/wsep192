using System;
using System.Collections.Generic;
using System.Text;
using DomainLayer.Data.Entitites;

namespace DomainLayer.Policies
{
    interface IPurchasePolicy
    {

        bool CheckPolicy(ShoppingCart shoppingCart);

    }
}
