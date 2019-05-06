﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DomainLayer.Operators
{
    public interface ILogicOperator
    {
        bool Operate(bool operand1, bool operand2);
    }
}
