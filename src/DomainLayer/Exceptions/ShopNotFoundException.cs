using System;

namespace DomainLayer.Exceptions
{
    public class ShopNotFoundException : Exception
    {
        public ShopNotFoundException(string message) : base(message) { }
    }
}
