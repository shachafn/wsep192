﻿using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using ApplicationCore.Interfaces.DataAccessLayer;
using DomainLayer.Operators;
using System;


namespace DomainLayer.Policies
{
    public class ProductDiscountPolicy : IDiscountPolicy
    {
        public Guid Guid { get; set; }
        public Guid ProductGuid { get; set; }
        public IArithmeticOperator Operator { get; set; }
        public int ExpectedQuantitiy { get; set; }
        public int DiscountPercentage { get; set; }
        public string Description { get; set; }


        public ProductDiscountPolicy(Guid productGuid, IArithmeticOperator @operator, int expectedQuantitiy, int discountPercentage, string description)
        {

            Guid = Guid.NewGuid();
            ProductGuid = productGuid;
            Operator = @operator;
            ExpectedQuantitiy = expectedQuantitiy;
            DiscountPercentage = discountPercentage;
            Description = description;
        }



        //Discount by percentage only!
        public bool CheckPolicy(ShoppingCart cart, Guid productGuid, int quantity, BaseUser user, IUnitOfWork unitOfWork)
        {
            foreach (Tuple<ShopProduct, int> sp in cart.PurchasedProducts)
            {
                if(sp.Item1.Guid.CompareTo(ProductGuid) == 0)
                   return Operator.IsValid(ExpectedQuantitiy, quantity);
            }
            return false;
        }
            private bool CheckPolicyHelper(ShoppingCart cart, Guid productGuid, int quantity, BaseUser user, IUnitOfWork unitOfWork)
        {
            return productGuid.CompareTo(ProductGuid) == 0 && Operator.IsValid(ExpectedQuantitiy, quantity);
        }

        public Tuple<ShopProduct, int> ApplyPolicy(ShoppingCart cart, Guid productGuid, int quantity
            , BaseUser user, IUnitOfWork unitOfWork)
        {
            foreach(Tuple<ShopProduct,int> sp in cart.PurchasedProducts) {
                if (!CheckPolicyHelper(cart, sp.Item1.Guid, sp.Item2, user, unitOfWork)) continue;
                Product p = null;
                Shop s = unitOfWork.ShopRepository.FindByIdOrNull(cart.ShopGuid);
                double shopProductPrice = 0;
                foreach (ShopProduct shopProduct in s.ShopProducts)
                {
                    if (shopProduct.Guid.CompareTo(sp.Item1.Guid) == 0)
                    {
                        p = shopProduct.Product;
                        shopProductPrice = shopProduct.Price;
                        break;
                    }
                }
                double discountValue = -(DiscountPercentage / 100.0) * shopProductPrice;
                if (discountValue == 0) return null;
                Product discountProduct = new Product("Discount - " + p.Name, "Discount");
                ShopProduct discountRecord = new ShopProduct(discountProduct, discountValue, 1);
                return new Tuple<ShopProduct, int>(discountRecord, quantity);
            }
            return null;
        }
    }
}
