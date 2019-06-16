using System;
using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using ApplicationCore.Interfaces.DataAccessLayer;
using DomainLayer.Operators;

namespace DomainLayer.Policies
{
    public class CompositeDiscountPolicy : IDiscountPolicy
    {
        public Guid Guid { get; set; }
        public IDiscountPolicy DiscountPolicy1 { get; set; }
        public ILogicOperator Operator { get; set; } //Discount can be conditioned by purchase
        public IDiscountPolicy DiscountPolicy2 { get; set; }
        public int DiscountPercentage { get; set; }
        public string Description { get; set; }

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
