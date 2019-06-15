using System;
using ApplicationCore.Entities.Users;
using ApplicationCore.Entitites;
using ApplicationCore.Interfaces.DataAccessLayer;
using DomainLayer.Operators;


namespace DomainLayer.Policies
{
    public class ProductPurchasePolicy : IPurchasePolicy
    {
        public Guid Guid { get; private set; }

        public int ExpectedQuantity { get; set; }
        public Guid ProductGuid { get; set; }
        public IArithmeticOperator Operator { get; set; }
        public string Description { get; set; }

        public ProductPurchasePolicy(Guid productGuid, IArithmeticOperator givenOperator, int expectedQuantity, string description)
        {
            Guid = Guid.NewGuid();

            ProductGuid = productGuid;
            Operator = givenOperator;
            ExpectedQuantity = expectedQuantity;
            Description = description;
        }

        public bool CheckPolicy(ShoppingCart cart, Guid productGuid, int quantity, BaseUser user, IUnitOfWork unitOfWork)
        {
            return productGuid.CompareTo(ProductGuid) == 0 ? Operator.IsValid(ExpectedQuantity, quantity) : true;
        }
    }
}

