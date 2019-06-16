using System;
using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using ApplicationCore.Interfaces.DataAccessLayer;
using DomainLayer.Operators;

namespace DomainLayer.Policies
{
    public class CartDiscountPolicy : IDiscountPolicy
    {
        public Guid Guid { get; set; }
        public double ExpectedSum { get; set; }
        public int DiscountPercentage { get; set; }
        public IArithmeticOperator Operator { get; set; }
        public string Description { get; set; }

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

        public bool CheckPolicy(ShoppingCart cart, Guid productGuid, int quantity, BaseUser user, IUnitOfWork unitOfWork)
        {
            double totalSum = CalculateSumBeforeDiscount(cart);
            return Operator.IsValid(ExpectedSum, totalSum);

        }
        private double CalculateSumBeforeDiscount(ShoppingCart cart)
        {
            double totalSum = 0;
            foreach (Tuple<ShopProduct, int> record in cart.PurchasedProducts)
            {
                if(record.Item1.Price > 0)
                    totalSum += (record.Item1.Price * record.Item2);
            }
            return totalSum;
        }

        public Tuple<ShopProduct, int> ApplyPolicy(ShoppingCart cart, Guid productGuid, int quantity, BaseUser user, IUnitOfWork unitOfWork)
        {
            if (CheckPolicy(cart, productGuid, quantity, user, unitOfWork))
            {
                double totalSum = CalculateSumBeforeDiscount(cart);
                double discountValue = -totalSum * (DiscountPercentage / 100.0);
                if (discountValue == 0) return null;
                Product discountProduct = new Product("Discount - cart", "Discount");
                var discountShopProduct = new ShopProduct(discountProduct, discountValue, 1);
                return new Tuple<ShopProduct, int>(discountShopProduct, 1);
            }
            return null;
        }
    }
}