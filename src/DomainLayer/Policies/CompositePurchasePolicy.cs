using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.Entities;
using DomainLayer.Data.Entitites;
using DomainLayer.Data.Entitites.Users;
using DomainLayer.Operators.LogicOperators;

namespace DomainLayer.Policies
{
    
    class CompositePurchasePolicy : IPurchasePolicy
    {
        private IPurchasePolicy PurchasePolicy1 { get; }
        private IPurchasePolicy PurchasePolicy2 { get; }
        private ILogicOperator Operator { get; }

        public bool CheckPolicy(ShoppingCart cart, Guid productGuid, int quantity, BaseUser user)
        {
            return Operator.Operate(PurchasePolicy1.CheckPolicy(cart, productGuid,quantity,user), PurchasePolicy2.CheckPolicy(cart, productGuid, quantity, user));
        }
    }
}
