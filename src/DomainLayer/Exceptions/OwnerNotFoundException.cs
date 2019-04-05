using System;

namespace DomainLayer.Exceptions
{
    public class OwnerNotFoundException : Exception
    {
        public OwnerNotFoundException(string message) : base(message) { }
    }
}
