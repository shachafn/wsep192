using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.Data;
using ApplicationCore.Entities;
using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using DomainLayer.Data.Entitites;
using DomainLayer.Data.Entitites.Users;
using DomainLayer.Operators;

namespace DomainLayer.Policies
{
    class CartPurchasePolicy : IPurchasePolicy
    {
        public Guid Guid { get; set; }
        private int ExpectedQuantity { get; }
        private IArithmeticOperator Operator { get; }
        public string Description { get; }


        public CartPurchasePolicy(int expectedQuantity, IArithmeticOperator @operator, string description)
        {
            Guid = Guid.NewGuid();
            ExpectedQuantity = expectedQuantity;
            Operator = @operator;
            Description = description;
        }

        public bool CheckPolicy(ShoppingCart cart, Guid productGuid, int quantity, BaseUser user)
        {
            return Operator.IsValid(ExpectedQuantity, GetCartSize(cart));
        }
        private int GetCartSize(ShoppingCart cart)
        {
            //Adding only products that are found in shop
            //Just in case someone applied Discount policy before Purchase policy
            int numberOfProducts = 0;
            foreach (Tuple<Guid, int> record in cart.PurchasedProducts)
            {
                Shop shop = DomainData.ShopsCollection[cart.ShopGuid];
                foreach (ShopProduct productInShop in shop.ShopProducts)
                {
                    if (productInShop.Guid.Equals(record.Item1))
                    {
                        numberOfProducts += record.Item2;
                        break;
                    }
                }
            }
            return numberOfProducts;
        }
    }
}
