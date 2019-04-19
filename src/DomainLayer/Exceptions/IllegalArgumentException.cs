using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLayer.Exceptions
{
    public class IllegalArgumentException : Exception, ICloneableException<Exception>
    {
        public IllegalArgumentException() { }

        public IllegalArgumentException(string msg) : base(msg) { }

        Exception ICloneableException<Exception>.Clone(string msg)
        {
            return new IllegalArgumentException(msg);
        }
    }
}
