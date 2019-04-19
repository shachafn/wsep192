using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLayer.Exceptions
{
    public class BrokenConstraintException : Exception, ICloneableException<Exception>
    {
        public BrokenConstraintException() { }

        public BrokenConstraintException(string msg) : base(msg) { }

        Exception ICloneableException<Exception>.Clone(string msg)
        {
            return new BrokenConstraintException(msg);
        }
    }
}
