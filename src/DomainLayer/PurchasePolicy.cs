using System;
using DomainLayer.ArithmeticOperators;
using DomainLayer.Data.Entitites;

namespace DomainLayer
{
    public class PurchasePolicy : IPurchasePolicy
    {
        private int Quantity { get; }
        private Guid ProductGuid { get; }
        private IArithmeticOperator ArithmeticOperator { get; }
        public PurchasePolicy()
        {
        }

        public bool CheckPolicy(ShoppingCart shoppingCart)
        {
            foreach (Tuple<Guid, int> record in shoppingCart.PurchasedProducts)
            {
                if (record.Item1.CompareTo(ProductGuid) == 0)
                    return ArithmeticOperator.IsValid(Quantity, record.Item2);
            }
            return true;
        }
    }
}

