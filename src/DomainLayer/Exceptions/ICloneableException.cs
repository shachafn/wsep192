using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLayer.Exceptions
{
    public interface ICloneableException<T> where T : Exception
    {
        T Clone(string msg);
    }
}
