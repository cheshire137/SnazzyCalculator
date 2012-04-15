using System;
using System.Collections.Generic;

namespace Calculator
{
	public class NoSpaceExpression : Symbol
	{
		public static NoSpaceExpression Produce(IEnumerable<Symbol> symbols)
		{
			return new NoSpaceExpression(symbols);
		}
 
		public NoSpaceExpression(params Object[] symbols) : base(symbols)
		{
		}
	}
}

