using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLayer.Exceptions
{
    public class ShopStateException : Exception
    {
        public ShopStateException(string msg) : base(msg) { }
    }
}
