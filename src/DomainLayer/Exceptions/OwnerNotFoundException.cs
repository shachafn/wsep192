using System;

namespace DomainLayer.Exceptions
{
    public class OwnerNotFoundException : Exception, ICloneableException<Exception>
    {
        public OwnerNotFoundException() { }

        public OwnerNotFoundException(string msg) : base(msg) { }

        Exception ICloneableException<Exception>.Clone(string msg)
        {
            return new OwnerNotFoundException(msg);
        }
    }
}
