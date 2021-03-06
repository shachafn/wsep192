﻿using System;

namespace ApplicationCore.Exceptions
{
    public class SystemAlreadyInitializedException : BaseException, ICloneableException<Exception>
    {
        public SystemAlreadyInitializedException() { }

        public SystemAlreadyInitializedException(string msg) : base(msg) { }

        Exception ICloneableException<Exception>.Clone(string msg)
        {
            return new SystemAlreadyInitializedException(msg);
        }
    }
}
