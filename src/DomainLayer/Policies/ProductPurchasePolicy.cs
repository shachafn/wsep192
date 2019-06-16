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

        private int ExpectedQuantity { get; }
        private Guid ProductGuid { get; }
        private IArithmeticOperator Operator { get; }
        public string Description { get; }

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
            foreach (Tuple<ShopProduct, int> sp in cart.PurchasedProducts)
            {
                if (sp.Item1.Guid.Equals(ProductGuid))
                    return Operator.IsValid(ExpectedQuantity, sp.Item2);
                //return productGuid.CompareTo(ProductGuid) == 0 ? Operator.IsValid(ExpectedQuantity, quantity) : true;
            }
            return false;
        }
    }
}

