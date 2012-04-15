using System;

namespace Calculator
{
	public class InfixOperator : Symbol
	{
		public static InfixOperator Produce(Symbol symbol)
		{
			// infix-operator = caret / asterisk / forward-slash / plus / minus
			if (symbol is Plus || symbol is Minus || symbol is Asterisk ||
			    symbol is ForwardSlash || symbol is Caret)
			{
				return new InfixOperator(symbol);
			}
			return null;
		}

		public InfixOperator(params Object[] symbols) : base(symbols)
		{
		}
		
		public Func<double, double, double> GetSolver()
		{
			Symbol sym = ConstituentSymbols[0];
			if (sym is Plus)
			{
				return (ad1, ad2) => ad1 + ad2;
			}
			if (sym is Minus)
			{
				return (t1, t2) => t1 - t2;
			}
			if (sym is Asterisk)
			{
				return (f1, f2) => f1 * f2;
			}
			if (sym is ForwardSlash)
			{
				return (dividend, divisor) => {
					if (0 == divisor) {
						throw new DivideByZeroException();
					}
					return dividend / divisor;
				};
			}
			if (sym is Caret)
			{
				return (t1, t2) => Math.Pow(t1, t2);
			}
			return (t1, t2) => {
				throw new ParserException("Don't know how to solve " +
				                          "expression with InfixOperator " +
				                          sym.GetType().ToString());
			};
		}
	}
}

