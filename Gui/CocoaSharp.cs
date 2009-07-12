#if OSX
using System;
using Cocoa;

namespace Gui
{
	public class CocoaSharp : IGui
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