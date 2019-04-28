using System;
using System.Collections.Generic;
using System.Text;
using DomainLayer.Data.Entitites;

namespace DomainLayer
{
    interface IPurchasePolicy
    {

        bool CheckPolicy(ShoppingCart shoppingCart);

    }
}
