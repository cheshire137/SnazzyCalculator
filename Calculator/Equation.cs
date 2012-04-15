using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace Calculator
{
	public class Equation
	{
		private readonly string _equation;
		public const char EXECUTE_OPERATOR = '=';
		public const char CLEAR_OPERATOR = 'C';
		public static List<char> Operators = new List<char> { '/', '+', '-', '*' };
        
		public Equation(string equation)
		{
			_equation = equation;
		}

		public bool HasFinalOperator()
		{
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
			return _equation.TrimEnd(Operators.ToArray()) + newOp;
		}

		public override string ToString()
		{
			return _equation;
		}
	}
}
