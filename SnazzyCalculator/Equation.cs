using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SnazzyCalculator
{
	public class Equation
	{
	    private readonly string _equation;
	    private readonly Queue<double> _values;
	    private readonly Queue<string> _operators;

        public Equation(string equation)
        {
            _equation = equation;
            _values = new Queue<double>();
            _operators = new Queue<string>();
        }

        public void Parse()
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
                else if (Calculator.Operators.Contains(strChar))
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

        public double Solve()
        {
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
                    throw new ArgumentException("Invalid operation; only " +
                        string.Join(", ", Calculator.Operators.ToArray()) + " are allowed");
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

        private static double subtract(double term1, double term2)
        {
            return term1 - term2;
        }
	}
}
