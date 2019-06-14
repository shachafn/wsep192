using System;

namespace ApplicationCore.Exceptions
{
    public class ExternalServiceFaultException : BaseException, ICloneableException<Exception>
    {
        public ExternalServiceFaultException() { }
        public ExternalServiceFaultException(string msg) :base(msg){ }
        Exception ICloneableException<Exception>.Clone(string msg)
        {
            return new ExternalServiceFaultException(msg);
        }
    }
}
