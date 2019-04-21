using System;

namespace DomainLayer.Exceptions
{
    public class SystemAlreadyInitializedException : Exception, ICloneableException<Exception>
    {
        public SystemAlreadyInitializedException() { }

        public SystemAlreadyInitializedException(string msg) : base(msg) { }

        Exception ICloneableException<Exception>.Clone(string msg)
        {
            return new SystemAlreadyInitializedException(msg);
        }
    }
}
