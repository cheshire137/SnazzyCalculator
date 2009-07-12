#if LINUX
using System;
using System.Collections.Generic;
using Calculator;
using Gtk;

namespace Gui
{
    public class GtkSharp : IGui
    {
        private static TextView _textBox;
        private static TextBuffer _buffer;
        private readonly Window _window;
        private Table _table;
        
        public GtkSharp()
        {
            Application.Init();
            _window = new Window(GuiData.TITLE);
            _window.Resize(GuiData.WIN_WIDTH, GuiData.WIN_HEIGHT);
        }

        public void DisplayMessage(NotificationType notifyType, string message)
        {
			MessageType mesType;
			
			if (NotificationType.Error == notifyType)
			{
				mesType = MessageType.Error;
			}
			else
			{
				mesType = MessageType.Info;
			}
			
			MessageDialog dialog = new MessageDialog(_window,
			                                         DialogFlags.DestroyWithParent,
			                                         mesType,
			                                         ButtonsType.Close,
			                                         message);
			dialog.Run();
			dialog.Destroy();
        }

        public void PopulateWindow()
        {
            _table = getTable();
            _window.Add(_table);
        }

        public void ShowWindow()
        {
            _table.ShowAll();
            _window.ShowAll();
            Application.Run();
        }

        private Table getTable()
        {
            Table table = new Table(GuiData.NUM_ROWS, GuiData.NUM_COLS, false);
            _textBox = new TextView();
            _buffer = _textBox.Buffer;
            _buffer.Text = "";

            uint col1 = 0, row1 = 0, col2 = GuiData.NUM_COLS - 1, row2 = 1;
            table.Attach(_textBox, col1, col2, row1, row2);

            foreach (KeyValuePair<string, uint[]> pair in GuiData.ButtonPlacements)
            {
                string numOrOp = pair.Key;
                Button button = new Button(numOrOp);

                // Clear
                if (Equation.CLEAR_OPERATOR == numOrOp[0])
                {
                    button.Clicked += clearButtonHandler;
                }
                // Math or execute operators
                else if (Equation.Operators.Contains(numOrOp) || Equation.EXECUTE_OPERATOR == numOrOp[0])
                {
                    button.Clicked += operatorButtonHandler;
                }
                // Numbers
                else
                {
                    button.Clicked += numButtonHandler;
                }

                uint[] placement = pair.Value;
                col1 = placement[0];
                row1 = placement[1];
                col2 = placement[2];
                row2 = placement[3];

                // Put the button in the table
                table.Attach(button, col1, col2, row1, row2);
            }

            return table;
        }

        private static void clearButtonHandler(object obj, EventArgs args)
        {
            _buffer.Text = String.Empty;
        }
        
        private static void numButtonHandler(object obj, EventArgs args)
        {
            Button button = (Button)obj;
            _buffer.Text += button.Label;
        }
        
        private void operatorButtonHandler(object obj, EventArgs args)
        {
            Button button = (Button)obj;
            char op = button.Label[0];
            Equation eq = new Equation(_buffer.Text);

            if (Equation.EXECUTE_OPERATOR == op)
            {
                if (eq.IsValid())
                {
                    double result = eq.Solve();
                    _buffer.Text += Equation.EXECUTE_OPERATOR + result.ToString();
                }
                else
                {
                    DisplayMessage(NotificationType.Error,
					               "Invalid equation " + _buffer.Text);
                }
            }
            else if (eq.HasFinalOperator())
            {
                _buffer.Text = eq.ReplaceFinalOperator(op);
            }
            else
            {
                _buffer.Text += op;
            }
        }
    }
}
#endif