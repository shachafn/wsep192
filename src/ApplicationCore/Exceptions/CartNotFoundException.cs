﻿using System;

namespace ApplicationCore.Exceptions
{
    public class CartNotFoundException : BaseException, ICloneableException<Exception>
    {
        public CartNotFoundException() { }

        public CartNotFoundException(string msg) : base(msg) { }

        Exception ICloneableException<Exception>.Clone(string msg)
        {
            return new CartNotFoundException(msg);
        }
    }
}
