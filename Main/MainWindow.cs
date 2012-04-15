using System;
using Gtk;
using Calculator;

public partial class MainWindow : Gtk.Window
{
	private const string ERROR_MESSAGE = "ERROR";
    private const string FONT_DESCRIPTION = "American Typewriter Regular 24";
    private Pango.Layout _equationLayout = null;
    private string _equationText = "0";
	
	public MainWindow() : base (Gtk.WindowType.Toplevel)
	{
		Build();
	}
	
	protected void OnDeleteEvent(object sender, DeleteEventArgs a)
	{
		Application.Quit();
		a.RetVal = true;
	}

	protected void OnButton7Clicked(object sender, System.EventArgs e)
	{
		numberButtonClicked(7);
	}
	
	protected void OnButton8Clicked(object sender, System.EventArgs e)
	{
		numberButtonClicked(8);
	}

	protected void OnButton9Clicked(object sender, System.EventArgs e)
	{
		numberButtonClicked(9);
	}

	protected void OnButtonDivideClicked(object sender, System.EventArgs e)
	{
		operatorButtonClicked('/');
	}

	protected void OnButton4Clicked(object sender, System.EventArgs e)
	{
		numberButtonClicked(4);
	}

	protected void OnButton5Clicked(object sender, System.EventArgs e)
	{
		numberButtonClicked(5);
	}

	protected void OnButton6Clicked(object sender, System.EventArgs e)
	{
		numberButtonClicked(6);
	}

	protected void OnButtonMultiplyClicked(object sender, System.EventArgs e)
	{
		operatorButtonClicked('*');
	}

	protected void OnButton1Clicked(object sender, System.EventArgs e)
	{
		numberButtonClicked(1);
	}

	protected void OnButton2Clicked(object sender, System.EventArgs e)
	{
		numberButtonClicked(2);
	}

	protected void OnButton3Clicked(object sender, System.EventArgs e)
	{
		numberButtonClicked(3);
	}

	protected void OnButtonSubtractClicked(object sender, System.EventArgs e)
	{
		operatorButtonClicked('-');
	}

	protected void OnButton0Clicked(object sender, System.EventArgs e)
	{
		numberButtonClicked(0);
	}

	protected void OnButtonDotClicked(object sender, System.EventArgs e)
	{
		if (ERROR_MESSAGE.Equals(_equationText))
        {
            changeEquationText("0.");
            return;
        }
        var eq = new Equation(_equationText);
        if (!eq.HasFinalOperator())
        {
            changeEquationText(_equationText + ".");
        }
	}

	protected void OnButtonAddClicked(object sender, System.EventArgs e)
	{
		operatorButtonClicked('+');
	}

	protected void OnButtonEqualsClicked(object sender, System.EventArgs e)
	{
		try
		{
            Formula formula = SimpleFormulaParser.ParseFormula(
                _equationText
            );
            changeEquationText("" + formula.Solve());
		}
		catch (ParserException ex)
		{
            changeEquationText(ERROR_MESSAGE);
			Console.WriteLine(ex.Message);
		}
		catch (DivideByZeroException)
		{
            changeEquationText(ERROR_MESSAGE);
		}
	}

	protected void OnButtonClearClicked(object sender, System.EventArgs e)
	{
        changeEquationText("0");
	}
	
	private void operatorButtonClicked(char op)
	{
		if (ERROR_MESSAGE.Equals(_equationText))
		{
            changeEquationText("0" + op);
			return;
		}
		var eq = new Equation(_equationText);
		if (eq.HasFinalOperator()) {
            changeEquationText(eq.ReplaceFinalOperator(op));
		}
		else
		{
            changeEquationText(_equationText + op);
		}
	}
	
	private void numberButtonClicked(int number)
	{
		if ("0".Equals(_equationText) ||
		    	ERROR_MESSAGE.Equals(_equationText)) {
            changeEquationText("" + number);
			return;
		}
        changeEquationText(_equationText + number);
	}

    protected void OnEquationAreaExposeEvent(object o, Gtk.ExposeEventArgs args)
    {
        changeEquationText(_equationText);
    }

    private void changeEquationText(string value)
    {
        if (null == _equationLayout)
        {
            _equationLayout = new Pango.Layout(this.PangoContext);
            _equationLayout.Wrap = Pango.WrapMode.Word;
            _equationLayout.Alignment = Pango.Alignment.Right;
            _equationLayout.FontDescription =
                Pango.FontDescription.FromString(FONT_DESCRIPTION);
        }
        _equationText = value;
        _equationLayout.SetMarkup(_equationText);
        equationArea.GdkWindow.Clear();
        equationArea.GdkWindow.DrawLayout(
            equationArea.Style.TextGC(StateType.Normal), 5, 5, _equationLayout
        );
    }

    protected void OnButtonBackspaceClicked(object sender, System.EventArgs e)
    {
        string valueAfterBackspace = _equationText.Remove(
            _equationText.Length - 1
        );
        if (string.IsNullOrEmpty(valueAfterBackspace))
        {
            valueAfterBackspace = "0";
        }
        changeEquationText(valueAfterBackspace);
    }
}
