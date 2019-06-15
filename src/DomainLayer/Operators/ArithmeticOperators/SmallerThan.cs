namespace DomainLayer.Operators
{
    class SmallerThan : IArithmeticOperator
    {
        public bool IsValid(double expected, double input)
        {
            return expected > input;
        }
        public override string ToString()
        {
            return "<";
        }
    }
}
