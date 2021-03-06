﻿using System;

namespace ApplicationCore.Exceptions
{
    public class ProductNotFoundException : BaseException, ICloneableException<Exception>
    {
        public ProductNotFoundException() { }

        public ProductNotFoundException(string msg) : base(msg) { }

        Exception ICloneableException<Exception>.Clone(string msg)
        {
            return new ProductNotFoundException(msg);
        }
    }
}
