using System;

namespace DomainLayer.Exceptions
{
    public class UserNotFoundException : Exception, ICloneableException<Exception>
    {
        public UserNotFoundException() { }

        public UserNotFoundException(string msg) : base(msg) { }

        Exception ICloneableException<Exception>.Clone(string msg)
        {
            return new UserNotFoundException(msg);
        }
    }
}
