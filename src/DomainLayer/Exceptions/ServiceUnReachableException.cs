using System;

namespace DomainLayer.Exceptions
{
    public class ServiceUnReachableException : Exception, ICloneableException<Exception>
    {
        public ServiceUnReachableException() { }

        public ServiceUnReachableException(string msg) : base(msg) { }

        Exception ICloneableException<Exception>.Clone(string msg)
        {
            return new ServiceUnReachableException(msg);
        }
    }
}
