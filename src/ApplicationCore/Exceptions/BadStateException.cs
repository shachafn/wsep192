﻿using System;

namespace ApplicationCore.Exceptions
{
    public class BadStateException : BaseException, ICloneableException<Exception>
    {
        public BadStateException() { }

        public BadStateException(string msg) : base(msg) { }

        Exception ICloneableException<Exception>.Clone(string msg)
        {
            return new BadStateException(msg);
        }
    }
}
