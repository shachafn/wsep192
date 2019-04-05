using System;

namespace DomainLayer.Exceptions
{
    public class BadStateException : Exception
    {
        public BadStateException(string message) : base(message) { }
    }
}
