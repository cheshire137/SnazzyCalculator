using System;
using System.Collections.Generic;
using System.Linq;
using Util;

namespace Calculator
{
	public class Expression : Symbol
	{
		public static Expression Produce(IEnumerable<Symbol> symbols)
		{
			// expression = *whitespace nospace-expression *whitespace
			int numSpaceBefore = symbols.TakeWhile(s => s is WhiteSpace).Count();
			int numSpaceAfter = symbols.Reverse()
				.TakeWhile(s => s is WhiteSpace).Count();
			IEnumerable<Symbol> noSpaceSymbolList = symbols
                .Skip(numSpaceBefore)
                .SkipLast(numSpaceAfter)
                .ToList();
			NoSpaceExpression n = NoSpaceExpression.Produce(noSpaceSymbolList);
			if (null == n)
			{
				return null;
			}
			return new Expression(WhiteSpace.CreateWhiteSpace(numSpaceBefore),
			                      n, WhiteSpace.CreateWhiteSpace(numSpaceAfter));
		}
 
		public Expression(params Object[] symbols) : base(symbols)
		{
		}
	}
}

