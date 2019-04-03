using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLayer.Exceptions
{
    public class BrokenConstraintException : Exception
    {
        public BrokenConstraintException(string message) : base(message) { }
    }
}
