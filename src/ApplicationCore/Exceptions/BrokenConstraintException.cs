﻿using System;

namespace ApplicationCore.Exceptions
{
    public class BrokenConstraintException : BaseException, ICloneableException<Exception>
    {
        public BrokenConstraintException() { }

        public BrokenConstraintException(string msg) : base(msg) { }

        Exception ICloneableException<Exception>.Clone(string msg)
        {
            return new BrokenConstraintException(msg);
        }
    }
}
