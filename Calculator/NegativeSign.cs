using System;

namespace Calculator
{
	public class NegativeSign : Symbol
	{
		public static NegativeSign Produce(Symbol symbol)
		{
			// neg-sign = minus
			if (symbol is Minus)
			{
				return new NegativeSign(symbol);
			}
			return null;
		}

		public NegativeSign(params Object[] symbols) : base(symbols)
		{
		}
	}
}

