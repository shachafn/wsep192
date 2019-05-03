using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using DomainLayer.Data.Entitites;
using DomainLayer.Data.Entitites.Users;
using DomainLayer.Operators.LogicOperators;

namespace DomainLayer.Policies
{
    class CompositeDiscountPolicy : IDiscountPolicy
    {
        private IDiscountPolicy DiscountPolicy1 { get; } 
        private IDiscountPolicy DiscountPolicy2 { get; }
        private Implies Operator { get; } //Discount can be conditioned by purchase
        public bool CheckPolicy(ref ShoppingCart cart, Guid productGuid, int quantity, BaseUser user)
        {
            return Operator.Operate(DiscountPolicy1.CheckPolicy(ref cart,productGuid,quantity,user), DiscountPolicy2.CheckPolicy(ref cart, productGuid, quantity, user));
        }
    }
}
