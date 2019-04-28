using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLayer.ArithmeticOperators
{
    interface IArithmeticOperator
    {
        bool IsValid(int expected,int input);
    }
}
