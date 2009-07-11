#if LINUX
using System;
using System.Collections.Generic;
using Gtk;

namespace SnazzyCalculator
{
    public class GtkSharp
    {
        private static TextView _textBox;
        private static TextBuffer _buffer;
        private readonly Window _window;
        private Table _table;
        
        public GtkSharp()
        {
            Application.Init();
            _window = new Window(MainWindow.TITLE);
            _window.Resize(MainWindow.WIN_WIDTH, MainWindow.WIN_HEIGHT);
        }

        public static void DisplayMessage(string title, string message)
        {
            Window win = new Window(title);
            win.SetDefaultSize(300, 200);
            Label label = new Label(message);
            win.Add(label);
            win.ShowAll();
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

        private static Table getTable()
        {
            Table table = new Table(Calculator.NUM_ROWS, Calculator.NUM_COLS, false);
            _textBox = new TextView();
            _buffer = _textBox.Buffer;
            _buffer.Text = "";

            uint col1 = 0, row1 = 0, col2 = Calculator.NUM_COLS, row2 = 1;
            table.Attach(_textBox, col1, col2, row1, row2);

            foreach (KeyValuePair<string, uint[]> pair in Calculator.ButtonPlacements)
            {
                string num = pair.Key;
                Button button = new Button(num);

                if (Calculator.Operators.Contains(num) || Calculator.EXECUTE_OPERATOR == num[0])
                {
                    button.Clicked += operatorButtonHandler;
                }
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
        
        private static void numButtonHandler(object obj, EventArgs args)
        {
            Button button = (Button)obj;
            _buffer.Text += button.Label;
        }
        
        private static void operatorButtonHandler(object obj, EventArgs args)
        {
            Button button = (Button)obj;
            char op = button.Label[0];
            
            if (Calculator.EXECUTE_OPERATOR == op)
            {
                if (Calculator.IsValidEquation(_buffer.Text))
                {
                    _buffer.Text = Calculator.Calculate(_buffer.Text);
                }
                else
                {
                    DisplayMessage("Error", "Invalid equation " + _buffer.Text);
                }
            }
            else if (Calculator.HasFinalOperator(_buffer.Text))
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