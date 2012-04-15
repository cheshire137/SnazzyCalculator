using System;

namespace Calculator
{
	public class DecimalDigit : Symbol
	{
		private string CharacterValue;

		public override string ToString()
		{
			return CharacterValue;
		}

		public DecimalDigit(char c)
		{
			CharacterValue = c.ToString();
		}
	}
}

