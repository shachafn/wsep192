using System;

namespace DomainLayer.Exceptions
{
    public class SystemNotInitializedException : Exception, ICloneableException<Exception>
    {
        public SystemNotInitializedException() { }

        public SystemNotInitializedException(string msg) : base(msg) { }

        Exception ICloneableException<Exception>.Clone(string msg)
        {
            return new SystemNotInitializedException(msg);
        }
    }
}
