﻿namespace DomainLayer.Operators
{
    public class Xor : ILogicOperator
    {
        public bool Operate(bool operand1, bool operand2)
        {
            return operand1 ? !operand2 : operand2;
        }
    }
}
