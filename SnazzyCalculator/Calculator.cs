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
            CocoaSharp gui = new CocoaSharp();
#elif LINUX
            GtkSharp gui = new GtkSharp();
#endif
            gui.PopulateWindow();
            gui.ShowWindow();
        }

        public static string Calculate(string equation)
        {
            string[] numbers = equation.Split(_charOperators);
            return "calculated";
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

            string joinedOps = string.Join("", Operators.ToArray());
            string pattern = @"(\d+[" + joinedOps + "])+";
            Regex valid = new Regex(pattern);
            Match m = valid.Match(equation);
            return m.Success;
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