using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLayer.Exceptions
{
    public class VerifierReflectionNotFound : Exception
    {
        public VerifierReflectionNotFound(string msg) : base(msg) { }
        public VerifierReflectionNotFound(string msg, Exception inner) : base(msg,inner) { }

    }
}
