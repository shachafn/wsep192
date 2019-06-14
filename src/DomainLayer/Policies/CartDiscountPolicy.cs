using System;
using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using DomainLayer.Operators;

namespace DomainLayer.Policies
{
    class CartDiscountPolicy : IDiscountPolicy
    {
        private Guid Guid { get; }
        private double ExpectedSum;
        public int DiscountPercentage { get; set; }
        private IArithmeticOperator Operator;
        public string Description { get; }

        Guid IDiscountPolicy.Guid => Guid;
        public CartDiscountPolicy(IArithmeticOperator @operator, double expectedSum, int discountpercentage, string description)
        {
            Guid = Guid.NewGuid();
            ExpectedSum = expectedSum;
            DiscountPercentage = discountpercentage;
            Operator = @operator;
            Description = description;
        }

        public CartDiscountPolicy()
        {
        }

        public bool CheckPolicy(ShoppingCart cart, Guid productGuid, int quantity, BaseUser user)
        {
            double totalSum = CalculateSumBeforeDiscount(cart);
            return Operator.IsValid(ExpectedSum, totalSum);

        }
        private double CalculateSumBeforeDiscount(ShoppingCart cart)
        {
            double totalSum = 0;
            foreach (Tuple<ShopProduct, int> record in cart.PurchasedProducts)
            {
                totalSum += (record.Item1.Price * record.Item2);
            }
            return totalSum;
        }

        public Tuple<ShopProduct, int> ApplyPolicy(ShoppingCart cart, Guid productGuid, int quantity, BaseUser user)
        {
            if (CheckPolicy(cart, productGuid, quantity, user))
            {
                double totalSum = CalculateSumBeforeDiscount(cart);
                double discountValue = -totalSum * (DiscountPercentage / 100);
                if (discountValue == 0) return null;
                Product discountProduct = new Product("Discount - cart", "Discount");
                var discountShopProduct = new ShopProduct(discountProduct, discountValue, 1);
                return new Tuple<ShopProduct, int>(discountShopProduct, 1);
            }
            return null;
        }
    }
}