using System;
using DomainLayer.Data.Entitites;

namespace DomainLayer
{
    public class PurchasePolicy : IPurchasePolicy
    {
        private int Quantity { get; }
        private Guid ProductGuid { get; }

        public PurchasePolicy()
        {
        }

        public bool CheckPolicy(ShoppingCart shoppingCart)
        {
            throw new NotImplementedException();
        }
    }
}

