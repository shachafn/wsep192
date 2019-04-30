using System;
using ApplicationCore.Entities;
using DomainLayer.Data.Entitites;
using DomainLayer.Data.Entitites.Users;
using DomainLayer.Operators.ArithmeticOperators;


namespace DomainLayer.Policies
{
    public class ProductPurchasePolicy : IPurchasePolicy
    {
        private int ExpectedQuantity { get; }
        private Guid ProductGuid { get; }
        private IArithmeticOperator Operator { get; }
        public ProductPurchasePolicy()
        {
        }

        public bool CheckPolicy(ShoppingCart cart, Guid productGuid, int quantity, BaseUser user)
        {
            return productGuid.CompareTo(ProductGuid) == 0 ? Operator.IsValid(ExpectedQuantity, quantity) : true ;
        }
    }
}

