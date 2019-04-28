using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLayer.Operators
{
    class Xor : IOperator
    {
        public bool Operate(bool operand1, bool operand2)
        {
            return operand1 ? !operand2 : operand2;
        }
    }
}
