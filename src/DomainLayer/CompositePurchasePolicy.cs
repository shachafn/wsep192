using System;
using System.Collections.Generic;
using System.Text;
using DomainLayer.Data.Entitites;

namespace DomainLayer
{
    
    class CompositePurchasePolicy : IPurchasePolicy
    {
        private IPurchasePolicy PurchasePolicy1;
        private IPurchasePolicy PurchasePolicy2;
        private ILogicOperator Operator;

        public bool CheckPolicy(ShoppingCart shoppingCart)
        {
            return Operator.Operate(PurchasePolicy1.CheckPolicy(shoppingCart), PurchasePolicy2.CheckPolicy(shoppingCart));
        }
    }
}
