using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SnazzyCalculator
{
    class Calculator
    {
        public const uint NUM_ROWS = 6;
        public const uint NUM_COLS = 4;
        public const char EXECUTE_OPERATOR = '=';
        
        public static List<string> Operators = new List<string> {"/", "+", "-", "*"};
        private static char[] _charOperators = getCharOperators();
        private static IGui _gui;

        public static Dictionary<string, uint[]> ButtonPlacements =
            new Dictionary<string, uint[]>
                {
                    {"/", new uint[] {0, 1, 2, 2}},
                    {"*", new uint[] {2, 1, 3, 2}},
                    {"-", new uint[] {3, 1, 4, 2}},
                    {"7", new uint[] {0, 2, 1, 3}},
                    {"8", new uint[] {1, 2, 2, 3}},
                    {"9", new uint[] {2, 2, 3, 3}},
                    {"+", new uint[] {3, 2, 4, 4}},
                    {"4", new uint[] {0, 3, 1, 4}},
                    {"5", new uint[] {1, 3, 2, 4}},
                    {"6", new uint[] {2, 3, 3, 4}},
                    {"1", new uint[] {0, 4, 1, 5}},
                    {"2", new uint[] {1, 4, 2, 5}},
                    {"3", new uint[] {2, 4, 3, 5}},
                    {"=", new uint[] {3, 4, 4, 6}},
                    {"0", new uint[] {0, 5, 2, 6}},
                    {".", new uint[] {2, 5, 3, 6}}
                };
        
        public static void Main(string[] args)
        {
#if OSX
            _gui = new CocoaSharp();
#elif LINUX
            _gui = new GtkSharp();
#endif
            _gui.PopulateWindow();
            _gui.ShowWindow();
        }

        public static string Calculate(string equation)
        {
            string[] strNumbers = equation.Split(_charOperators);
            Regex numberRegex = new Regex(@"\d");
            Queue<double> values = new Queue<double>();
            string curValue = String.Empty;
            Queue<string> ops = new Queue<string>();

            foreach (char c in equation)
            {
                string strChar = c.ToString();

                if (numberRegex.Match(strChar).Success)
                {
                    curValue += strChar;
                }
                else if (Operators.Contains(strChar))
                {
                    // Once we hit an operator, add it to the list of operators
                    // and store the current number in the list of values, then
                    // wipe the curValue buffer
                    ops.Enqueue(strChar);
                    double value = Convert.ToDouble(curValue);
                    values.Enqueue(value);
                    curValue = String.Empty;
                }
            }

            if (!string.IsNullOrEmpty(curValue))
            {
                double value = Convert.ToDouble(curValue);
                values.Enqueue(value);
            }

            double result = values.Dequeue();

            while (values.Count > 0)
            {
                string op = ops.Dequeue();
                double value = values.Dequeue();

                if ("+" == op)
                {
                    result += value;
                }
                else if ("-" == op)
                {
                    result -= value;
                }
                else if ("*" == op)
                {
                    result *= value;
                }
                else if ("/" == op)
                {
                    result /= value;
                }
                else
                {
                    throw new ArgumentException("Invalid operation; only " +
                        string.Join(", ", Operators.ToArray()) + " are allowed");
                }
            }

            return result.ToString();
        }
        
        public static bool HasFinalOperator(string text)
        {
            bool result = false;
            
            foreach (string op in Operators)
            {
                if (text.EndsWith(op))
                {
                    result = true;
                    break;
                }
            }
            
            return result;
        }

        public static bool IsValidEquation(string equation)
        {
            bool result = false;

            // Ensure the equation uses at least one operator
            foreach (string op in Operators)
            {
                result = result || equation.Contains(op);

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
            Regex valid = null;

            try
            {
                valid = new Regex(pattern);
            }
            catch (System.ArgumentException ex)
            {
                _gui.DisplayMessage("Exception", ex.Message);
            }

            if (null != valid)
            {
                Match m = valid.Match(equation);
                return m.Success;
            }

            return false;
        }
        
        public static string ReplaceFinalOperator(string text, char newOp)
        {
            return text.TrimEnd(_charOperators) + newOp;
        }

        private static char[] getCharOperators()
        {
            char[] ops = new char[Operators.Count];
            int index = 0;

            foreach (string op in Operators)
            {
                ops[index] = op[0];
                index++;
            }

            return ops;
        }
    }
}