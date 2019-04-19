using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLayer.Exceptions
{
    public class IllegalOperationException : Exception, ICloneableException<Exception>
    {
        public IllegalOperationException() { }

        public IllegalOperationException(string msg) : base(msg) { }

        Exception ICloneableException<Exception>.Clone(string msg)
        {
            return new IllegalOperationException(msg);
        }
    }
}
