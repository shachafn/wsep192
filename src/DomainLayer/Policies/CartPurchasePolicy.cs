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
        public Guid Guid { get; set; }
        private int ExpectedQuantity { get; }
        private IArithmeticOperator Operator { get; }
        private string Description{ get; }


        public CartPurchasePolicy(string name,int expectedQuantity, IArithmeticOperator @operator,string description)
        {
            Guid = Guid.NewGuid();
            ExpectedQuantity = expectedQuantity;
            Operator = @operator;
            Description = description;
        }

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
