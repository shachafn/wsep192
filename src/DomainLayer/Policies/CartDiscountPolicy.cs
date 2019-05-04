using System;
using ApplicationCore.Data;
using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using DomainLayer.Operators.ArithmeticOperators;

namespace DomainLayer.Policies
{
    class CartDiscountPolicy : IDiscountPolicy
    {
        private double ExpectedSum;
        private int Discountpercentage;
        private IArithmeticOperator Operator;

        public CartDiscountPolicy(double expectedSum, int discountpercentage, IArithmeticOperator @operator)
        {
            ExpectedSum = expectedSum;
            Discountpercentage = discountpercentage;
            Operator = @operator;
        }

        public bool CheckPolicy(ref ShoppingCart cart, Guid productGuid, int quantity, BaseUser user)
        {
            double totalSum = 0;
            foreach(Tuple<Guid,int> record in cart.PurchasedProducts)
            {
                Shop shop = DomainData.ShopsCollection[cart.ShopGuid];
                foreach(ShopProduct productInShop in shop.ShopProducts)
                {
                    if (productInShop.Equals(record.Item1))
                    {
                        totalSum += (productInShop.Quantity * (double)record.Item2);
                        break;
                    }
                }
            }
            if (Operator.IsValid(ExpectedSum, totalSum))
            {
                Product discountProdct = new Product("Discount - cart", "Discount");
                ShopProduct discountRecord = new ShopProduct(discountProdct, -totalSum * (Discountpercentage / 100), 1);
                var discountRecordGuid = discountProdct.Guid;
                Tuple<Guid, int> newRecordToCart = new Tuple<Guid, int>(discountRecordGuid, 1);
                cart.PurchasedProducts.Add(newRecordToCart);
                return true;
            }
            return false;
        }
    }
}