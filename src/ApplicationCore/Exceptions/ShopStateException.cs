using System;

namespace ApplicationCore.Exceptions
{
    public class ShopStateException : BaseException, ICloneableException<Exception>
    {
        public ShopStateException() { }

        public ShopStateException(string msg) : base(msg) { }

        Exception ICloneableException<Exception>.Clone(string msg)
        {
            return new ShopStateException(msg);
        }
    }
}
