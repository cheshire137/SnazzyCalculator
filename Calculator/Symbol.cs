using System;
using System.Collections.Generic;
using Util;
using System.Linq;
using System.Text;

namespace Calculator
{
	public abstract class Symbol
	{
		public Symbol()
		{
			ConstituentSymbols = new List<Symbol>();
		}
		
		public Symbol(params Object[] symbols) : this()
		{
			foreach (var item in symbols)
			{
				if (item is Symbol)
				{
					ConstituentSymbols.Add((Symbol)item);
				}
				else if (item is IEnumerable<Symbol>)
				{
					foreach (Symbol item2 in (IEnumerable<Symbol>)item)
					{
						ConstituentSymbols.Add(item2);
					}
				}
				else
				{
					// If this error is thrown, the parser is coded incorrectly.
					string msg = "Expected Symbol or IEnumerable<Symbol>";
					if (null != item)
					{
						msg += ", instead got " + item.GetType().ToString();
					}
					throw new ParserException(msg);
				}
			}
		}
		
		public List<Symbol> ConstituentSymbols { get; set; }
		
		public String Inspect()
		{
			var sb = new StringBuilder();
			sb.AppendLine(GetType().ToString());
			foreach (Symbol sym in ConstituentSymbols)
			{
				sb.Append(sym.GetType().ToString())
					.Append(" - ")
					.AppendLine(sym.Inspect());
			}
			return sb.ToString();
		}

		public override string ToString()
		{
			return ConstituentSymbols.Select(ct => ct.ToString())
				.StringConcatenate();
		}
	}
}

