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
		
		public double Solve()
		{
			int numSymbols = ConstituentSymbols.Count;
			if (1 == numSymbols)
			{
				Symbol sym = ConstituentSymbols[0];
				if (sym is WholeNumberPart)
				{
					return ((WholeNumberPart)sym).Solve();
				}
				else if (sym is FractionalPart)
				{
					return ((FractionalPart)sym).Solve();
				}
				else
				{
					throw new ParserException("Don't know how to solve " +
					    "SignificandPart with no FractionalPart or " +
					    "WholeNumberPart, but instead a " +
					    sym.GetType().ToString());
				}
			}
			if (2 != numSymbols)
			{
				throw new ParserException("Expected WholeNumberPart and " +
				                          "FractionalPart, but have " +
				                          numSymbols + " symbols");
			}
			// TODO: is this right?
			var w = (WholeNumberPart)ConstituentSymbols[0];
			var f = (FractionalPart)ConstituentSymbols[1];
			return w.Solve() + f.Solve();
		}
	}
}

