using System;

namespace ApplicationCore.Exceptions
{
    public class NoPrivilegesException : BaseException, ICloneableException<Exception>
    {
        public NoPrivilegesException() { }

        public NoPrivilegesException(string msg) : base(msg) { }

        Exception ICloneableException<Exception>.Clone(string msg)
        {
            return new NoPrivilegesException(msg);
        }
    }
}
