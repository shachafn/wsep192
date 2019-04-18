using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLayer.Exceptions
{
    public class ProductNotFoundException : Exception, ICloneableException<Exception>
    {
        public ProductNotFoundException() { }

        public ProductNotFoundException(string msg) : base(msg) { }

        Exception ICloneableException<Exception>.Clone(string msg)
        {
            return new ProductNotFoundException(msg);
        }
    }
}
