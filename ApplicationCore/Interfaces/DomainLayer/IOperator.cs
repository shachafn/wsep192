using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLayer
{
    public interface ILogicOperator
    {
        bool Operate(bool operand1, bool operand2);
    }
}
