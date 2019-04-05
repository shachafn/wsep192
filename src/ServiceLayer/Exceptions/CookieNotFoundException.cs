using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceLayer.Exceptions
{
    public class CookieNotFoundException : Exception
    {
        public CookieNotFoundException(string message) : base(message) { }
    }
}
