#if LINUX
using System;
using System.Collections.Generic;
using System.Linq;
using Gtk;

namespace SnazzyCalculator
{
	public class GtkSharp
	{
		private static Gtk.TextView _textBox;
		private static Gtk.TextBuffer _buffer;
		
		public GtkSharp()
		{
			Application.Init();
			Window win = new Window(MainWindow.TITLE);
			win.Resize(MainWindow.WIN_WIDTH, MainWindow.WIN_HEIGHT);
			
            Table table = new Table(Calculator.NUM_ROWS, Calculator.NUM_COLS, false);
			_textBox = new Gtk.TextView();
			_buffer = _textBox.Buffer;
			_buffer.Text = "";
			
			uint col1 = 0, row1 = 0, col2 = Calculator.NUM_COLS, row2 = 1;
            table.Attach(_textBox, col1, col2, row1, row2);
			
			foreach (KeyValuePair<string, uint[]> pair in Calculator.ButtonPlacements)
			{
                string num = pair.Key;
				Button button = new Button(num);
				
				if (Calculator.Operators.Contains(num))
				{
					button.Clicked += new EventHandler(operatorButtonHandler);
				}
				else
				{
					button.Clicked += new EventHandler(numButtonHandler);
				}
				
				uint[] placement = pair.Value;
                col1 = placement[0];
                row1 = placement[1];
                col2 = placement[2];
                row2 = placement[3];
                
				// Put the button in the table
				table.Attach(button, col1, col2, row1, row2);
			}
			
			table.ShowAll();
			win.Add(table);
			win.ShowAll();
			Application.Run();
		}
		
		static void numButtonHandler(object obj, EventArgs args)
		{
			Button button = (Button)obj;
			_buffer.Text += (string)button.Label;
		}
		
		static void operatorButtonHandler(object obj, EventArgs args)
		{
			Button button = (Button)obj;
			string op = (string)button.Label;
			
			if (Calculator.HasFinalOperator(_buffer.Text))
			{
				_buffer.Text = Calculator.ReplaceFinalOperator(_buffer.Text, op);
			}
			else
			{
				_buffer.Text += op;
			}
		}
	}
}
#endif