using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLayer.Operators.ArithmeticOperators
{
    class SmallerThan : IArithmeticOperator
    {
        public bool IsValid(int expected, int input)
        {
            return expected > input;
        }
    }
}
