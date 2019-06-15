using System;

namespace ApplicationCore.Exceptions
{
    public class BaseException : Exception
    {
        public BaseException(string msg, Exception ex) : base(msg, ex) { }
        public BaseException(string msg) : base(msg) { }
        public BaseException() : base() { }
    }
}
