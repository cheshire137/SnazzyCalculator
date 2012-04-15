using System;

namespace Calculator
{
	public class DecimalDigit : Symbol
	{
		private string _charValue;

		public override string ToString()
		{
			return _charValue;
		}

		public DecimalDigit(char c)
		{
			_charValue = c.ToString();
		}
		
		public double Solve()
		{
			return Double.Parse(_charValue);
		}
	}
}

