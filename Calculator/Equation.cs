using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace Calculator
{
	public class Equation
	{
		private readonly string _equation;
		public static List<char> Operators = new List<char> { '/', '+', '-',
            '*', '.' };
        
		public Equation(string equation)
		{
			_equation = equation;
		}

		public bool HasFinalOperator()
		{
            if (string.IsNullOrEmpty(_equation))
            {
                return false;
            }
			bool result = false;
			foreach (char op in Operators) {
				if (_equation.EndsWith(op.ToString())) {
					result = true;
					break;
				}
			}
			return result;
		}
		
		public string ReplaceFinalOperator(char newOp)
		{
            if (string.IsNullOrEmpty(_equation))
            {
                return newOp.ToString();
            }
			return _equation.TrimEnd(Operators.ToArray()) + newOp;
		}

		public override string ToString()
		{
			return _equation;
		}
	}
}
