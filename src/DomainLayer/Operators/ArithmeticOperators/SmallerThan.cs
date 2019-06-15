﻿namespace DomainLayer.Operators
{
    public class SmallerThan : IArithmeticOperator
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
