using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLayer.ArithmeticOperators
{
    class BiggerThan : IArithmeticOperator
    {
        public bool IsValid(int expected, int input)
        {
            return expected < input;
        }
    }
}
