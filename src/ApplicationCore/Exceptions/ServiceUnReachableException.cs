﻿using System;

namespace ApplicationCore.Exceptions
{
    public class ServiceUnReachableException : BaseException, ICloneableException<Exception>
    {
        public ServiceUnReachableException() { }

        public ServiceUnReachableException(string msg) : base(msg) { }

        Exception ICloneableException<Exception>.Clone(string msg)
        {
            return new ServiceUnReachableException(msg);
        }
    }
}
