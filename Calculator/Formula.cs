using System;
using System.Collections.Generic;

namespace Calculator
{
	public class Formula : Symbol
	{
		public static Formula Produce(IEnumerable<Symbol> symbols)
		{
			// formula = expression
			Expression e = Expression.Produce(symbols);
			return e == null ? null : new Formula(e);
		}

		public Formula(params Object[] symbols) : base(symbols)
		{
		}
	}
}

