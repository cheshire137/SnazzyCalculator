using System;
using System.Collections.Generic;
using Gtk;

namespace SnazzyCalculator
{
	public class GtkSharp
	{
		private static Gtk.TextView _textBox;
		
		public GtkSharp()
		{
			Application.Init();
			Window win = new Window(MainWindow.TITLE);
			win.Resize(MainWindow.WIN_WIDTH, MainWindow.WIN_HEIGHT);
			
			Table table = new Table(Calculator.NUM_ROWS, Calculator.NUM_COLS, false);
			uint rowIndex1 = 0;
			uint colIndex1 = 0;
			uint rowIndex2 = 1;
			uint colIndex2 = Calculator.NUM_COLS;
			_textBox = new Gtk.TextView();
			_textBox.Buffer.Text = "Hello";
			
			table.Attach(_textBox, colIndex1, colIndex2, rowIndex1, rowIndex2);
			rowIndex1++;
			rowIndex2++;
			colIndex2 = 1;
			
			Dictionary<string, uint[]> buttonPlacements = Calculator.GetButtonPlacements(
				rowIndex1, colIndex1, rowIndex2, colIndex2
			);
			
			foreach (string num in Calculator.NUM_PAD)
			{
				Button button = new Button(num);
				button.Clicked += new EventHandler(numButtonHandler);
				uint[] placement = buttonPlacements[num];
				
				// Put the button in the table
				table.Attach(button, placement[0], placement[1], placement[2], placement[3]);
			}
			
			table.ShowAll();
			win.Add(table);
			win.ShowAll();
			Application.Run();
		}
		
		static void numButtonHandler(object obj, EventArgs args)
		{
			Button button = (Button)obj;
			_textBox.Buffer.Text = (string)button.Label;
		}
	}
}
