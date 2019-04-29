using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLayer.ArithmeticOperators
{
    public interface IArithmeticOperator
    {
        bool IsValid(int expected,int input);
    }
}
