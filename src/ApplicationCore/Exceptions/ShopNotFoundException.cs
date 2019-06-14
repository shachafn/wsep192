using System;

namespace ApplicationCore.Exceptions
{
    public class ShopNotFoundException : BaseException, ICloneableException<Exception>
    {
        public ShopNotFoundException() { }

        public ShopNotFoundException(string msg) : base(msg) { }

        Exception ICloneableException<Exception>.Clone(string msg)
        {
            return new ShopNotFoundException(msg);
        }
    }
}
