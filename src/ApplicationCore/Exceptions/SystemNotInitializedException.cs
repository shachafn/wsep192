using System;

namespace ApplicationCore.Exceptions
{
    public class SystemNotInitializedException : BaseException, ICloneableException<Exception>
    {
        public SystemNotInitializedException() { }

        public SystemNotInitializedException(string msg) : base(msg) { }

        Exception ICloneableException<Exception>.Clone(string msg)
        {
            return new SystemNotInitializedException(msg);
        }
    }
}
