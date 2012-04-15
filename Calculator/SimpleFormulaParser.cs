using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Calculator
{
    public class SimpleFormulaParser
    {
        public static Formula ParseFormula(string s)
        {
            IEnumerable<Symbol> symbols = s.Select(charToSymbol);
            /*if (symbols.Any())
            {
                Console.WriteLine("Terminal Symbols");
                Console.WriteLine("================");
                foreach (Symbol terminal in symbols)
                {
                    Console.WriteLine("{0} >{1}<",
                        terminal.GetType().Name.ToString(),
                        terminal.ToString());
                }
                Console.WriteLine();
            }*/
            Formula formula = Formula.Produce(symbols);
            if (null == formula)
            {
                throw new ParserException("Invalid formula");
            }
            return formula;
        }

        private static Symbol charToSymbol(char c)
        {
            switch (c)
            {
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    return new DecimalDigit(c);
                case ' ':
                case '\t':
                case '\r':
                case '\n':
                    return new WhiteSpace();
                case '+':
                    return new Plus();
                case '-':
                    return new Minus();
                case '*':
                    return new Asterisk();
                case '/':
                    return new ForwardSlash();
                case '^':
                    return new Caret();
                case '.':
                    return new Period();
                case '(':
                    return new OpenParenthesis();
                case ')':
                    return new ClosedParenthesis();
                default:
                    return (Symbol)null;
            }
        }
    }
}

