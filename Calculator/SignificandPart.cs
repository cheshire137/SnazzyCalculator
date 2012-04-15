using System;
using System.Collections.Generic;
using System.Linq;

namespace Calculator
{
	public class SignificandPart : Symbol
	{
		public static SignificandPart Produce(IEnumerable<Symbol> symbols)
		{
			// significand-part = fractional-part / whole-number-part [fractional-part]
			FractionalPart f;
			f = FractionalPart.Produce(symbols);
			if (f != null)
			{
				return new SignificandPart(f);
			}
			IEnumerable<Symbol> s = null;
			WholeNumberPart w = WholeNumberPart.Produce(symbols, out s);
			if (w != null)
			{
				if (!s.Any())
				{
					return new SignificandPart(w);
				}
				f = FractionalPart.Produce(s);
				if (f != null)
				{
					return new SignificandPart(w, f);
				}
			}
			return null;
		}

		public SignificandPart(params Object[] symbols) : base(symbols)
		{
		}
	}
}

