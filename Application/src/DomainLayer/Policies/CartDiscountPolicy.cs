using System;
using ApplicationCore.Data;
using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using DomainLayer.Extension_Methods;
using DomainLayer.Operators;

namespace DomainLayer.Policies
{
    class CartDiscountPolicy : IDiscountPolicy
    {
        private Guid Guid { get; }
        private double ExpectedSum;
        public int DiscountPercentage { get; set; }
        private IArithmeticOperator Operator;
        private string Description { get; }

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

        public bool CheckPolicy(ref ShoppingCart cart, Guid productGuid, int quantity, BaseUser user)
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

        public void ApplyPolicy(ref ShoppingCart cart, Guid productGuid, int quantity, BaseUser user)
        {
            if (CheckPolicy(ref cart, productGuid, quantity, user))
            {
                double totalSum = CalculateSumBeforeDiscount(cart);
                double discountValue = -totalSum * (DiscountPercentage / 100);
                if (discountValue == 0) return;
                Product discountProduct = new Product("Discount - cart", "Discount");
                ShopProduct discountRecord = new ShopProduct(discountProduct, discountValue, 1);
                cart.AddProductToCart(discountRecord, 1);
            }
        }
    }
}