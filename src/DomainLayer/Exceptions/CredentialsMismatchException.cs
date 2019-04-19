using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLayer.Exceptions
{
    public class CredentialsMismatchException : Exception, ICloneableException<Exception>
    {
        public CredentialsMismatchException() { }

        public CredentialsMismatchException(string msg) : base(msg) { }

        Exception ICloneableException<Exception>.Clone(string msg)
        {
            return new CredentialsMismatchException(msg);
        }
    }
}
