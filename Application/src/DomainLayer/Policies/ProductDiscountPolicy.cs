﻿using ApplicationCore.Data;
using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using ApplicationCore.Interfaces.DAL;
using DomainLayer.Extension_Methods;
using DomainLayer.Operators;
using System;


namespace DomainLayer.Policies
{
    class ProductDiscountPolicy : IDiscountPolicy
    {
        public Guid Guid { get; private set; }
        private Guid ProductGuid { get; }
        private IArithmeticOperator Operator { get; }
        private int ExpectedQuantitiy { get; }
        public int DiscountPercentage { get; set; }
        private string Description { get; }

        private IUnitOfWork _unitOfWork;
        public ProductDiscountPolicy(Guid productGuid, IArithmeticOperator @operator, int expectedQuantitiy, int discountPercentage, string description, IUnitOfWork unitOfWork)
        {

            Guid = Guid.NewGuid();
            ProductGuid = productGuid;
            Operator = @operator;
            ExpectedQuantitiy = expectedQuantitiy;
            DiscountPercentage = discountPercentage;
            Description = description;
            _unitOfWork = unitOfWork;
        }



        //Discount by percentage only!
        public bool CheckPolicy(ref ShoppingCart cart, Guid productGuid, int quantity, BaseUser user)
        {
            return productGuid.CompareTo(ProductGuid) == 0 && Operator.IsValid(ExpectedQuantitiy, quantity);
        }

        public void ApplyPolicy(ref ShoppingCart cart, Guid productGuid, int quantity, BaseUser user)
        {
            if (CheckPolicy(ref cart, productGuid, quantity, user))
            {
                Product p = null;
                Shop s = _unitOfWork.ShopRepository.FindById(cart.ShopGuid);
                double shopProductPrice = 0;
                foreach (ShopProduct shopProduct in s.ShopProducts)
                {
                    if (s.Guid.CompareTo(productGuid) == 0)
                    {
                        p = shopProduct.Product;
                        shopProductPrice = shopProduct.Price;
                        break;
                    }
                }
                double discountValue = -(DiscountPercentage / 100) * shopProductPrice;
                if (discountValue == 0) return;
                Product discountProduct = new Product("Discount - " + p.Name, "Discount");
                ShopProduct discountRecord = new ShopProduct(discountProduct, discountValue, 1);
                cart.AddProductToCart(discountRecord.Guid, quantity);
            }
        }
    }
}
