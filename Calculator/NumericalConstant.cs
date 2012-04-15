using System;
using System.Collections.Generic;
using System.Linq;

namespace Calculator
{
	public class NumericalConstant : Symbol
	{
		public static NumericalConstant Produce(IEnumerable<Symbol> symbols)
		{
			// numerical-constant = [neg-sign] significand-part
			SignificandPart s = SignificandPart.Produce(symbols);
			if (s != null)
			{
				return new NumericalConstant(s);
			}
			NegativeSign n = NegativeSign.Produce(symbols.First());
			if (n != null)
			{
				SignificandPart s2 = SignificandPart.Produce(symbols.Skip(1));
				if (s2 != null)
				{
					return new NumericalConstant(n, s2);
				}
			}
			return null;
		}

		public NumericalConstant(params Object[] symbols) : base(symbols)
		{
		}
		
		public double Solve()
		{
			if (ConstituentSymbols.Count == 1)
			{
				return ((SignificandPart)ConstituentSymbols[0]).Solve();
			}
			return Double.Parse(
				((NegativeSign)ConstituentSymbols[0]).ToString() +
				((SignificandPart)ConstituentSymbols[1]).Solve()
			);
		}
	}
}

