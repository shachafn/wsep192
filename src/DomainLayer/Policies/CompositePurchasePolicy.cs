using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.Entities;
using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using DomainLayer.Data.Entitites;
using DomainLayer.Data.Entitites.Users;
using DomainLayer.Operators.LogicOperators;

namespace DomainLayer.Policies
{
    
    class CompositePurchasePolicy : IPurchasePolicy
    {
        public Guid Guid { get; private set; }
        private IPurchasePolicy PurchasePolicy1 { get; }
        private IPurchasePolicy PurchasePolicy2 { get; }
        private ILogicOperator Operator { get; }
        private string Description { get; }


        public CompositePurchasePolicy(IPurchasePolicy purchasePolicy1, IPurchasePolicy purchasePolicy2, ILogicOperator @operator, string description)
        {
            Guid = Guid.NewGuid();
            PurchasePolicy1 = purchasePolicy1;
            PurchasePolicy2 = purchasePolicy2;
            Operator = @operator;
            Description = description;
        }

        public bool CheckPolicy(ShoppingCart cart, Guid productGuid, int quantity, BaseUser user)
        {
            return Operator.Operate(PurchasePolicy1.CheckPolicy(cart, productGuid,quantity,user), PurchasePolicy2.CheckPolicy(cart, productGuid, quantity, user));
        }
    }
}
