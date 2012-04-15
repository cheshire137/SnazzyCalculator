using System;
using System.Collections.Generic;

namespace Calculator
{
	public class WholeNumberPart : Symbol
	{
		public static WholeNumberPart Produce(IEnumerable<Symbol> symbols,
        	out IEnumerable<Symbol> symbolsToProcess)
		{
			// whole-number-part = digit-sequence
			IEnumerable<Symbol> unprocessedSymbols = null;
			DigitSequence d = DigitSequence.Produce(symbols,
				out unprocessedSymbols);
			if (d != null)
			{
				symbolsToProcess = unprocessedSymbols;
				return new WholeNumberPart(d);
			}
			symbolsToProcess = null;
			return null;
		}

		public WholeNumberPart(params Object[] symbols) : base(symbols)
		{
		}
		
		public double Solve()
		{
			return ((DigitSequence)ConstituentSymbols[0]).Solve();
		}
	}
}

