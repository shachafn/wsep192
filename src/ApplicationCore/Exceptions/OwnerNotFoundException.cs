using System;

namespace ApplicationCore.Exceptions
{
    public class OwnerNotFoundException : BaseException, ICloneableException<Exception>
    {
        public OwnerNotFoundException() { }

        public OwnerNotFoundException(string msg) : base(msg) { }

        Exception ICloneableException<Exception>.Clone(string msg)
        {
            return new OwnerNotFoundException(msg);
        }
    }
}
