using System;
using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using DomainLayer.Operators;

namespace DomainLayer.Policies
{

    class CompositePurchasePolicy : IPurchasePolicy
    {
        public Guid Guid { get; private set; }
        private IPurchasePolicy PurchasePolicy1 { get; }
        private ILogicOperator Operator { get; }
        private IPurchasePolicy PurchasePolicy2 { get; }
        public string Description { get; }


        public CompositePurchasePolicy(IPurchasePolicy purchasePolicy1, ILogicOperator @operator, IPurchasePolicy purchasePolicy2, string description)
        {
            Guid = Guid.NewGuid();
            PurchasePolicy1 = purchasePolicy1;
            Operator = @operator;
            PurchasePolicy2 = purchasePolicy2;
            Description = description;
        }

        public bool CheckPolicy(ShoppingCart cart, Guid productGuid, int quantity, BaseUser user)
        {
            return Operator.Operate(PurchasePolicy1.CheckPolicy(cart, productGuid, quantity, user), PurchasePolicy2.CheckPolicy(cart, productGuid, quantity, user));
        }
    }
}
