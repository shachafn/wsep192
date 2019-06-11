using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLayer.Operators
{
    public interface IArithmeticOperator
    {
        bool IsValid(double expected,double input);
        
    }
}
