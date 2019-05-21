using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Exceptions
{
    public class ExternalServiceFaultException : Exception, ICloneableException<Exception>
    {
        public ExternalServiceFaultException() { }
        public ExternalServiceFaultException(string msg) :base(msg){ }
        Exception ICloneableException<Exception>.Clone(string msg)
        {
            return new ExternalServiceFaultException(msg);
        }
    }
}
