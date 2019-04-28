using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLayer
{
    interface IOperator
    {
        bool Operate(bool operand1, bool operand2);
    }
}
