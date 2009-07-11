#if OSX
using System;
using Cocoa;

namespace SnazzyCalculator
{
	public class CocoaSharp
	{
		private const string NIB_FILE = "snazzy_calculator.nib";
		
		public CocoaSharp()
		{
			Application.Init();
			Application.LoadNib(NIB_FILE);
			Application.Run();
		}
	}
}
#endif