﻿using System;

namespace ApplicationCore.Exceptions
{
    public class VerifierReflectionNotFound : BaseException
    {
        public VerifierReflectionNotFound(string msg) : base(msg) { }
        public VerifierReflectionNotFound(string msg, Exception inner) : base(msg,inner) { }

    }
}
