using System;

namespace ApplicationCore.Exceptions
{
    public class PolicyNotFoundException : Exception, ICloneableException<Exception>
    {
        public PolicyNotFoundException() { }

        public PolicyNotFoundException(string msg) : base(msg) { }

        Exception ICloneableException<Exception>.Clone(string msg)
        {
            return new PolicyNotFoundException(msg);
        }
    }
}
