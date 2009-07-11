using System;
using System.Collections.Generic;

namespace SnazzyCalculator
{
	class Calculator
	{
		public const uint NUM_ROWS = 5;
		public const uint NUM_COLS = 3;
		public static string[] NUM_PAD = new[]
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

			foreach (string num in NUM_PAD)
			{
				// Make the penultimate button stretch 2 spaces--it's the zero
				if (count == NUM_PAD.Length - 1)
				{
					colIndex2 = NUM_COLS - 1;
				}
				
				output.Add(num, new[] {colIndex1, colIndex2, rowIndex1, rowIndex2});

				colIndex1++;
				colIndex2++;
				
				// Handle the period
				if (count == NUM_PAD.Length - 1)
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
	}
}