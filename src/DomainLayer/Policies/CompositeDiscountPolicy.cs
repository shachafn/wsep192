using System;
using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using ApplicationCore.Interfaces.DataAccessLayer;
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

        public bool CheckPolicy(ShoppingCart cart, Guid productGuid, int quantity, BaseUser user, IUnitOfWork unitOfWork)
        {
            return Operator.Operate(DiscountPolicy1.CheckPolicy(cart, productGuid, quantity, user, unitOfWork), DiscountPolicy2.CheckPolicy(cart, productGuid, quantity, user, unitOfWork));
        }

        public Tuple<ShopProduct, int> ApplyPolicy(ShoppingCart cart, Guid productGuid, int quantity, BaseUser user, IUnitOfWork unitOfWork)
        {
            if (CheckPolicy(cart, productGuid, quantity, user, unitOfWork))
            {
                int prevDiscountPercentage = DiscountPolicy2.DiscountPercentage;
                DiscountPolicy2.DiscountPercentage = DiscountPercentage;
                var discountProductAndQuantity = DiscountPolicy2.ApplyPolicy(cart, productGuid, quantity, user, unitOfWork);
                DiscountPolicy2.DiscountPercentage = prevDiscountPercentage;
                return discountProductAndQuantity;
            }
            return null;
        }
    }
}
