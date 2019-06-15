namespace DomainLayer.Operators
{
    class And : ILogicOperator
    {
        public bool Operate(bool operand1, bool operand2)
        {
            return operand1 && operand2;
        }
    }
}
