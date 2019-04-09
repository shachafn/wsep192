using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLayer.Exceptions
{
    public class ProductNotFoundException : Exception
    {
        public ProductNotFoundException(string msg) : base(msg) { }
    }
}
