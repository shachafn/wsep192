using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLayer.Operators.LogicOperators
{
    class And : ILogicOperator
    {
        public bool Operate(bool operand1, bool operand2)
        {
            return operand1 && operand2;
        }
    }
}
