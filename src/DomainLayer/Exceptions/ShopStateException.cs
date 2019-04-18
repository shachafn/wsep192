using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLayer.Exceptions
{
    public class ShopStateException : Exception, ICloneableException<Exception>
    {
        public ShopStateException() { }

        public ShopStateException(string msg) : base(msg) { }

        Exception ICloneableException<Exception>.Clone(string msg)
        {
            return new ShopStateException(msg);
        }
    }
}
