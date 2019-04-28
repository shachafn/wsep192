using System;

namespace ServiceLayer.Exceptions
{
    public class CookieNotFoundException : Exception
    {
        public CookieNotFoundException(string message) : base(message) { }
    }
}
