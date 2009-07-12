using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace Calculator
{
	public class Equation
	{
	    private readonly string _equation;
	    private readonly Queue<double> _values;
	    private readonly Queue<string> _operators;

        public const char EXECUTE_OPERATOR = '=';
	    public const char CLEAR_OPERATOR = 'C';
        public static List<char> Operators = new List<char> { '/', '+', '-', '*' };
        
        public Equation(string equation)
        {
            _equation = equation;
            _values = new Queue<double>();
            _operators = new Queue<string>();
        }

        public bool HasFinalOperator()
        {
            bool result = false;

            foreach (char op in Operators)
            {
                if (_equation.EndsWith(op.ToString()))
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        public bool IsValid()
        {
            bool result = false;

            // Ensure the equation uses at least one operator
            foreach (char op in Operators)
            {
                result = result || _equation.Contains(op.ToString());

                if (result)
                {
                    break;
                }
            }

            // If no operators were found, return false now--invalid equation
            if (!result)
            {
                return false;
            }

            string pattern = @"(\d+[\+\*\-\\])+\d+";
            Regex valid = new Regex(pattern);
            Match m = valid.Match(_equation);
            return m.Success;
        }

        public string ReplaceFinalOperator(char newOp)
        {
            return _equation.TrimEnd(Operators.ToArray()) + newOp;
        }

        public double Solve()
        {
            parse();
            double result = _values.Dequeue();

            while (_values.Count > 0)
            {
                string op = _operators.Dequeue();
                double value = _values.Dequeue();
                Func<double, double, double> operation;

                if ("+" == op)
                {
                    operation = add;
                }
                else if ("-" == op)
                {
                    operation = subtract;
                }
                else if ("*" == op)
                {
                    operation = multiply;
                }
                else if ("/" == op)
                {
                    operation = divide;
                }
                else
                {
					string[] validOps = Operators.Select(curOp => curOp.ToString()).ToArray();
                    throw new ArgumentException("Invalid operation; only " +
                        string.Join(", ", validOps) + " are allowed");
                }

                result = operation.Invoke(result, value);
            }

            return result;
        }

        public override string ToString()
        {
            return _equation;
        }

        private static double add(double addend1, double addend2)
        {
            return addend1 + addend2;
        }

        private static double divide(double dividend, double divisor)
        {
            if (0 == divisor)
            {
                throw new DivideByZeroException();
            }

            return dividend / divisor;
        }

        private static double multiply(double factor1, double factor2)
        {
            return factor1 * factor2;
        }

        private void parse()
        {
            Regex numberRegex = new Regex(@"\d");
            string curValue = String.Empty;

            foreach (char c in _equation)
            {
                string strChar = c.ToString();

                if (numberRegex.Match(strChar).Success)
                {
                    curValue += strChar;
                }
                else if (Operators.Contains(c))
                {
                    // Once we hit an operator, add it to the list of operators
                    // and store the current number in the list of values, then
                    // wipe the curValue buffer
                    _operators.Enqueue(strChar);
                    double value = Convert.ToDouble(curValue);
                    _values.Enqueue(value);
                    curValue = String.Empty;
                }
            }

            if (!string.IsNullOrEmpty(curValue))
            {
                double value = Convert.ToDouble(curValue);
                _values.Enqueue(value);
            }
        }

        private static double subtract(double term1, double term2)
        {
            return term1 - term2;
        }
	}
}
