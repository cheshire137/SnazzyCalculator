using System;
using System.Collections.Generic;
using Util;
using System.Linq;

namespace Calculator
{
	public abstract class Symbol
	{
		public List<Symbol> ConstituentSymbols { get; set; }

		public override string ToString()
		{
			return ConstituentSymbols.Select(ct => ct.ToString())
				.StringConcatenate();
		}

		public Symbol(params Object[] symbols)
		{
			ConstituentSymbols = new List<Symbol>();
			foreach (var item in symbols)
			{
				if (item is Symbol)
				{
					ConstituentSymbols.Add((Symbol)item);
				}
				else if (item is IEnumerable<Symbol>)
				{
					foreach (var item2 in (IEnumerable<Symbol>)item)
					{
						ConstituentSymbols.Add(item2);
					}
				}
				else
				{
					// If this error is thrown, the parser is coded incorrectly.
					throw new ParserException("Internal error");
				}
			}
		}

		public Symbol()
		{
		}
	}
}

