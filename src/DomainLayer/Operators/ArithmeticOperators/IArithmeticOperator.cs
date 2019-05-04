using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLayer.Operators.ArithmeticOperators
{
    public interface IArithmeticOperator
    {
        bool IsValid(double expected,double input);
        
    }
}
