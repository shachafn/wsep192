using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLayer.Exceptions
{
    public class CartNotFoundException : Exception
    {
        public CartNotFoundException(string msg) : base(msg) { }
    }
}
