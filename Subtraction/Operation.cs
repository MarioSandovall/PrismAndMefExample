using Shared;
using System;
using System.ComponentModel.Composition;

namespace Subtraction
{
    [Export(typeof(IOperation))]
    public class Operation : IOperation
    {
        public int Op(string num1, string num2)
        {
            return Convert.ToInt32(num1) - Convert.ToInt32(num2);
        }
    }
}
