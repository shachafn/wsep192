using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.Entities;
using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using DomainLayer.Data.Entitites;
using DomainLayer.Data.Entitites.Users;
using DomainLayer.Operators.ArithmeticOperators;

namespace DomainLayer.Policies
{
    class CartPurchasePolicy : IPurchasePolicy
    {
        private int ExpectedQuantity { get; }
        private IArithmeticOperator Operator { get; }
        //private Func<ShoppingCart, int> ExtractInformation { get; }
        
        public CartPurchasePolicy(int expectedQuantity, IArithmeticOperator @operator, Func<ShoppingCart, int> extractInformation)
        {
            ExpectedQuantity = expectedQuantity;
            Operator = @operator;
            //ExtractInformation = extractInformation;
        }

        public bool CheckPolicy(ShoppingCart cart, Guid productGuid, int quantity, BaseUser user)
        {
            throw new NotImplementedException();
            //return Operator.IsValid(ExpectedQuantity, ExtractInformation(cart));
        }
    }
}
