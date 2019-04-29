using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.Interfaces.DomainLayer;

namespace DomainLayer.LogicOperators
{
    class Implies : ILogicOperator
    {
        public bool Operate(bool operand1, bool operand2)
        {
            return operand1 ? operand2 : true; 
        }
    }
}
