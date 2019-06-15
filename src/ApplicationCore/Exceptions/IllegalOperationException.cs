using System;

namespace ApplicationCore.Exceptions
{
    public class IllegalOperationException : BaseException, ICloneableException<Exception>
    {
        public IllegalOperationException() { }

        public IllegalOperationException(string msg) : base(msg) { }

        Exception ICloneableException<Exception>.Clone(string msg)
        {
            return new IllegalOperationException(msg);
        }
    }
}
