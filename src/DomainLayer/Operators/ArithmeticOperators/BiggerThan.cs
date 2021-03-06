﻿namespace DomainLayer.Operators
{
    public class BiggerThan : IArithmeticOperator
    {
        public bool IsValid(double expected, double input)
        {
            return expected < input;
        }

        public override string ToString()
        {
            return ">";
        }
    }
}
