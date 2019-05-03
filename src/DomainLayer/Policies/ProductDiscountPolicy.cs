using DomainLayer.Data;
using DomainLayer.Data.Entitites;
using DomainLayer.Data.Entitites.Users;
using DomainLayer.Operators.ArithmeticOperators;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLayer.Policies
{
    class ProductDiscountPolicy : IDiscountPolicy
    {
        private Guid ProductGuid { get; }
        private IArithmeticOperator Operator { get; }
        private int ExpectedQuantitiy { get; }
        private int DiscountPercentage { get; }

        //Discount by percentage only!
        public bool CheckPolicy(ref ShoppingCart cart, Guid productGuid, int quantity, BaseUser user)
        {
            if(productGuid.CompareTo(ProductGuid) == 0 && Operator.IsValid(ExpectedQuantitiy,quantity))
            {
                Product p = null;
                Shop s = DomainData.ShopsCollection[cart.ShopGuid];
                double shopProductPrice = 0;
                foreach(ShopProduct shopProduct in s.ShopProducts)
                {
                    if (s.Guid.CompareTo(productGuid) == 0)
                    {
                        p = shopProduct.Product;
                        shopProductPrice = shopProduct.Price;
                        break;
                    }
                }
                ShopProduct discountRecord = new ShopProduct(p, DiscountPercentage * shopProductPrice, 1);
                cart.AddProductToCart(discountRecord.Guid,quantity);
                return true;
            }
            return false;
        }
    }
}
