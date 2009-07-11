using System;
using System.Collections.Generic;

namespace SnazzyCalculator
{
	class Calculator
	{
		public const uint NUM_ROWS = 6;
		public const uint NUM_COLS = 4;
		
		public static List<string> Operators = new List<string> {"/", "+", "-", "*"};
		/*public static string[] NumPad = new[]
										 {
											 "/", "*", "-", "+", // rowIndex 0-1
										 	 "7", "8", "9",      // rowIndex 1-2
											 "4", "5", "6", "=", // rowIndex 2-3
											 "1", "2", "3",      // rowIndex 3-4
											 "0", "."            // rowIndex 4-5
										 };*/
        public static Dictionary<string, uint[]> ButtonPlacements =
            new Dictionary<string, uint[]>
                {
                    {"/", new uint[] {0, 1, 2, 2}},
                    {"*", new uint[] {2, 1, 3, 2}},
                    {"-", new uint[] {3, 1, 4, 2}},
                    {"7", new uint[] {0, 2, 1, 3}},
                    {"8", new uint[] {1, 2, 2, 3}},
                    {"9", new uint[] {2, 2, 3, 3}},
                    {"+", new uint[] {3, 2, 4, 4}},
                    {"4", new uint[] {0, 3, 1, 4}},
                    {"5", new uint[] {1, 3, 2, 4}},
                    {"6", new uint[] {2, 3, 3, 4}},
                    {"1", new uint[] {0, 4, 1, 5}},
                    {"2", new uint[] {1, 4, 2, 5}},
                    {"3", new uint[] {2, 4, 3, 5}},
                    {"=", new uint[] {3, 4, 4, 6}},
                    {"0", new uint[] {0, 5, 2, 6}},
                    {".", new uint[] {2, 5, 3, 6}}
                };
        
		public static void Main(string[] args)
		{
#if OSX
			CocoaSharp gui = new CocoaSharp();
#elif LINUX
			GtkSharp gui = new GtkSharp();
#endif
		}
		
		/*public static Dictionary<string, uint[]> GetButtonPlacements(
			uint rowIndex1, uint colIndex1, uint rowIndex2, uint colIndex2
		)
		{
			Dictionary<string, uint[]> output = new Dictionary<string, uint[]>();
			int count = 1;

			foreach (string num in NumPad)
			{
                // Handle tall plus button
                if (0 == rowIndex1 && 3 == colIndex1)
                {
                    rowIndex2++;
                }
                
				// Make the penultimate button stretch 2 spaces--it's the zero
				if (count == NumPad.Length - 1)
				{
					colIndex2 = NUM_COLS - 1;
				}
				
				output.Add(num, new[] {colIndex1, colIndex2, rowIndex1, rowIndex2});

				colIndex1++;
				colIndex2++;
				
                // Handle tall plus button
                if (0 == rowIndex1 && 3 == colIndex1)
                {
                    rowIndex1++;
                }
                
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
		}*/
		
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
				ops[index] = op[0];
				index++;
			}
			
			return text.TrimEnd(ops) + newOp;
		}
	}
}