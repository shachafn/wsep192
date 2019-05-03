using System;

namespace ApplicationCore.Exceptions
{
    public class NoPriviligesException : Exception, ICloneableException<Exception>
    {
        public NoPriviligesException() { }

        public NoPriviligesException(string msg) : base(msg) { }

        Exception ICloneableException<Exception>.Clone(string msg)
        {
            return new NoPriviligesException(msg);
        }
    }
}
