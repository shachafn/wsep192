using System;

namespace ApplicationCore.Exceptions
{
    public interface ICloneableException<T> where T : Exception
    {
        T Clone(string msg);
    }
}
