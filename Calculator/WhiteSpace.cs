using System;
using System.Collections.Generic;

namespace Calculator
{
	public class WhiteSpace : Symbol
	{
		public override string ToString()
		{
			return " ";
		}

		public WhiteSpace()
		{
		}
		
		public static IEnumerable<Symbol> CreateWhiteSpace(int amount)
		{
			List<Symbol> ws = new List<Symbol>();
			for (int i=0; i<amount; i++)
			{
				ws.Add(new WhiteSpace());
			}
			return (IEnumerable<Symbol>)ws;
		}
	}
}

