using System;

namespace DomainLayer.Exceptions
{
    public class ShopNotFoundException : Exception, ICloneableException<Exception>
    {
        public ShopNotFoundException() { }

        public ShopNotFoundException(string msg) : base(msg) { }

        Exception ICloneableException<Exception>.Clone(string msg)
        {
            return new ShopNotFoundException(msg);
        }
    }
}
