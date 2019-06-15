using System;

namespace ApplicationCore.Exceptions
{
    public class UserNotFoundException : BaseException, ICloneableException<Exception>
    {
        public UserNotFoundException() { }

        public UserNotFoundException(string msg) : base(msg) { }

        Exception ICloneableException<Exception>.Clone(string msg)
        {
            return new UserNotFoundException(msg);
        }
    }
}
