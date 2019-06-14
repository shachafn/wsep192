using System;
using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using DomainLayer.Operators;

namespace DomainLayer.Policies
{
    class CompositeDiscountPolicy : IDiscountPolicy
    {
        public Guid Guid { get; private set; }
        private IDiscountPolicy DiscountPolicy1 { get; }
        private ILogicOperator Operator { get; } //Discount can be conditioned by purchase
        private IDiscountPolicy DiscountPolicy2 { get; }
        public int DiscountPercentage { get; set; }
        public string Description { get; }

        public CompositeDiscountPolicy(IDiscountPolicy discountPolicy1, ILogicOperator @operator, IDiscountPolicy discountPolicy2, int discountPercentage, string description)
        {
            Guid = Guid.NewGuid();
            DiscountPolicy1 = discountPolicy1;
            Operator = @operator;
            DiscountPolicy2 = discountPolicy2;
            DiscountPercentage = discountPercentage;
            Description = description;
        }

        public bool CheckPolicy(ShoppingCart cart, Guid productGuid, int quantity, BaseUser user)
        {
            return Operator.Operate(DiscountPolicy1.CheckPolicy(cart, productGuid, quantity, user), DiscountPolicy2.CheckPolicy(cart, productGuid, quantity, user));
        }

        public Tuple<ShopProduct, int> ApplyPolicy(ShoppingCart cart, Guid productGuid, int quantity, BaseUser user)
        {
            if (CheckPolicy(cart, productGuid, quantity, user))
            {
                int prevDiscountPercentage = DiscountPolicy2.DiscountPercentage;
                DiscountPolicy2.DiscountPercentage = DiscountPercentage;
                var deiscountProductAndQuantity = DiscountPolicy2.ApplyPolicy(cart, productGuid, quantity, user);
                DiscountPolicy2.DiscountPercentage = prevDiscountPercentage;
                return deiscountProductAndQuantity;
            }
            return null;
        }
    }
}
