namespace DomainLayer.Operators
{
    public class Implies : ILogicOperator
    {
        public bool Operate(bool operand1, bool operand2)
        {
            return operand1 ? operand2 : true; 
        }
    }
}
