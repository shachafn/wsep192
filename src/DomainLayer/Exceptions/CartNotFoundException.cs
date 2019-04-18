using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLayer.Exceptions
{
    public class CartNotFoundException : Exception, ICloneableException<Exception>
    {
        public CartNotFoundException() { }

        public CartNotFoundException(string msg) : base(msg) { }

        Exception ICloneableException<Exception>.Clone(string msg)
        {
            return new CartNotFoundException(msg);
        }
    }
}
