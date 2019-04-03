using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLayer.Exceptions
{
    public class NoPriviligesException : Exception
    {
        public NoPriviligesException(string message) : base(message) { }
    }
}
