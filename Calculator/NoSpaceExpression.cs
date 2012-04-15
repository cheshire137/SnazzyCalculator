using System;
using System.Collections.Generic;
using System.Linq;
using Util;

namespace Calculator
{
	public class NoSpaceExpression : Symbol
	{
		public static Dictionary<string, int> OperatorPrecedence =
			new Dictionary<string, int>
				{
				    { "^", 3 },
				    { "*", 2 },
				    { "/", 2 },
				    { "+", 1 },
				    { "-", 1 },
				};
		
		public static NoSpaceExpression Produce(IEnumerable<Symbol> symbols)
		{
			// nospace-expression = open-parenthesis expression close-parenthesis
			//         / numerical-constant
			//         / prefix-operator expression
			//         / expression infix-operator expression
			if (!symbols.Any())
			{
				return null;
			}
			if (symbols.First() is OpenParenthesis &&
			    symbols.Last() is ClosedParenthesis)
			{
				Expression e = Expression.Produce(symbols.Skip(1).SkipLast(1));
				if (e != null)
				{
					return new NoSpaceExpression(new OpenParenthesis(), e,
					                             new ClosedParenthesis());
				}
			}
			// expression, infix-operator, expression
			var z = symbols.Rollup(0, (t, d) =>
			{
				if (t is OpenParenthesis)
				{
					return d + 1;
				}
				if (t is ClosedParenthesis)
				{
					return d - 1;
				}
				return d;
			});
			var symbolsWithIndex = symbols.Select((s, i) => new
			    {
			        Symbol = s,
			        Index = i,
			    });
			var z2 = symbolsWithIndex.Zip(z, (v1, v2) => new
			    {
			        SymbolWithIndex = v1,
			        Depth = v2,
			    });
			var operatorList = z2.Where(x =>
			    x.Depth == 0 &&
            	x.SymbolWithIndex.Index != 0 &&
            	InfixOperator.Produce(x.SymbolWithIndex.Symbol) != null
			).ToList();
			if (operatorList.Any())
			{
				int minPrecedence = operatorList.Select(o2 =>
					OperatorPrecedence[o2.SymbolWithIndex.Symbol.ToString()]
				).Min();
				var op = operatorList.Last(o2 =>
					OperatorPrecedence[o2.SymbolWithIndex.Symbol.ToString()] ==
				        minPrecedence
				);
				if (op != null)
				{
					var expressionTokenList1 = symbols.TakeWhile(t =>
						t != op.SymbolWithIndex.Symbol);
					Expression e1 = Expression.Produce(expressionTokenList1);
					if (e1 == null)
					{
						throw new ParserException("Invalid expression");
					}
					var expressionTokenList2 = symbols.SkipWhile(t =>
						t != op.SymbolWithIndex.Symbol).Skip(1);
					Expression e2 = Expression.Produce(expressionTokenList2);
					if (e2 == null)
					{
						throw new ParserException("Invalid expression");
					}
					InfixOperator io = new InfixOperator(op.SymbolWithIndex.Symbol);
					return new NoSpaceExpression(e1, io, e2);
				}
			}
			NumericalConstant n = NumericalConstant.Produce(symbols);
			if (n != null)
			{
				return new NoSpaceExpression(n);
			}
			PrefixOperator p = PrefixOperator.Produce(symbols.FirstOrDefault());
			if (p != null)
			{
				Expression e = Expression.Produce(symbols.Skip(1));
				if (e != null)
				{
					return new NoSpaceExpression(p, e);
				}
			}
			return null;
		}
 
		public NoSpaceExpression(params Object[] symbols) : base(symbols)
		{
		}
		
		public double Solve()
		{
			int numSymbols = ConstituentSymbols.Count;
			Symbol firstSym = ConstituentSymbols[0];
			if (1 == numSymbols)
			{
				return ((NumericalConstant)firstSym).Solve();
			}
			Symbol secondSym = ConstituentSymbols[1];
			if (2 == numSymbols)
			{
				return Double.Parse(
					((PrefixOperator)firstSym).ToString() +
					((Expression)secondSym).Solve()
				);
			}
			Symbol thirdSym = ConstituentSymbols[2];
			if (firstSym is Expression)
			{
				double term1 = ((Expression)firstSym).Solve();
				double term2 = ((Expression)thirdSym).Solve();
				return ((InfixOperator)secondSym).GetSolver().Invoke(term1, term2);
			}
			return ((Expression)secondSym).Solve();
		}
	}
}

