using System;

namespace ApplicationCore.Exceptions
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
