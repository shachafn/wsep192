using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLayer.Operators
{
    class BiggerThan : IArithmeticOperator
    {
        public bool IsValid(double expected, double input)
        {
            return expected < input;
        }

        public override string ToString()
        {
            return ">";
        }
    }
}
