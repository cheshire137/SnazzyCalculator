using System;
using System.Collections.Generic;
using System.Linq;

namespace Calculator
{
	public class FractionalPart : Symbol
	{
		public static FractionalPart Produce(IEnumerable<Symbol> symbols)
		{
			// fractional-part = period digit-sequence
			if (!symbols.Any())
			{
				return null;
			}
			if (symbols.First() is Period)
			{
				IEnumerable<Symbol> s = null;
				DigitSequence d = DigitSequence.Produce(symbols.Skip(1), out s);
				if (d == null || s.Any())
				{
					return null;
				}
				return new FractionalPart(new Period(), d);
			}
			return null;
		}

		public FractionalPart(params Object[] symbols) : base(symbols)
		{
		}
	}
}

