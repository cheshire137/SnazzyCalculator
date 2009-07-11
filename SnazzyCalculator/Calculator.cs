using System;
using System.Collections.Generic;

namespace SnazzyCalculator
{
	class Calculator
	{
		public const uint NUM_ROWS = 5;
		public const uint NUM_COLS = 3;
		
		public static List<string> Operators = new List<string> {"/", "+", "-", "*" };
		public static string[] NumPad = new[]
										 {
											 "/", "*", "-",
										 	 "7", "8", "9",
											 "4", "5", "6",
											 "1", "2", "3",
											 "0", "."
										 };
		
		public static void Main(string[] args)
		{
#if OSX
			CocoaSharp gui = new CocoaSharp();
#elif LINUX
			GtkSharp gui = new GtkSharp();
#endif
		}
		
		public static Dictionary<string, uint[]> GetButtonPlacements(
			uint rowIndex1, uint colIndex1, uint rowIndex2, uint colIndex2
		)
		{
			Dictionary<string, uint[]> output = new Dictionary<string, uint[]>();
			int count = 1;

			foreach (string num in NumPad)
			{
				// Make the penultimate button stretch 2 spaces--it's the zero
				if (count == NumPad.Length - 1)
				{
					colIndex2 = NUM_COLS - 1;
				}
				
				output.Add(num, new[] {colIndex1, colIndex2, rowIndex1, rowIndex2});

				colIndex1++;
				colIndex2++;
				
				// Handle the period
				if (count == NumPad.Length - 1)
				{
					colIndex1 = NUM_COLS - 1;
					colIndex2 = NUM_COLS;
				}
				
				// Restart at leftmost column when we've filled all columns left-to-right
				if (count % NUM_COLS == 0)
				{
					rowIndex1++;
					rowIndex2++;
					colIndex1 = 0;
					colIndex2 = 1;
				}
				
				count++;
			}
			
			return output;
		}
		
		public static bool HasFinalOperator(string text)
		{
			bool result = false;
			
			foreach (string op in Operators)
			{
				if (text.EndsWith(op))
				{
					result = true;
					break;
				}
			}
			
			return result;
		}
		
		public static string ReplaceFinalOperator(string text, string newOp)
		{
			char[] ops = new char[Operators.Count];
			int index = 0;
			
			foreach (string op in Operators)
			{
				ops[index] = op.ToCharArray(0, 1)[0];
				index++;
			}
			
			char oldOp = text[text.LastIndexOfAny(ops)];
		}
	}
}