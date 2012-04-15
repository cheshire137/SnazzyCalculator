using System;
using Gtk;
using Calculator;

public partial class MainWindow : Gtk.Window
{
	private const string ERROR_MESSAGE = "ERROR";
    private const string FONT_DESCRIPTION = "American Typewriter Regular 26";
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
		operatorButtonClicked(new ForwardSlash());
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
		operatorButtonClicked(new Asterisk());
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
		operatorButtonClicked(new Minus());
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
		operatorButtonClicked(new Plus());
	}

    private void solveEquation()
    {
        try
        {
            Formula formula = SimpleFormulaParser.ParseFormula(
                _equationText
            );
            Console.WriteLine(formula.Inspect());
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

	protected void OnButtonEqualsClicked(object sender, System.EventArgs e)
	{
        solveEquation();
	}

    private void clearEquationText()
    {
        changeEquationText("0");
    }

	protected void OnButtonClearClicked(object sender, System.EventArgs e)
	{
        clearEquationText();
	}
	
	private void operatorButtonClicked(Symbol op)
	{
		if (ERROR_MESSAGE.Equals(_equationText))
		{
            changeEquationText("0" + op.ToString());
			return;
		}
		var eq = new Equation(_equationText);
		if (eq.HasFinalOperator()) {
            changeEquationText(eq.ReplaceFinalOperator(op));
		}
		else
		{
            changeEquationText(_equationText + op.ToString());
		}
	}
	
	private void numberButtonClicked(int number)
	{
		if ("0".Equals(_equationText) ||
		    	ERROR_MESSAGE.Equals(_equationText)) {
            changeEquationText("" + number);
			return;
		}
        char lastChar = _equationText[_equationText.Length - 1];
        if (new ClosedParenthesis().ToString().Equals(lastChar.ToString()))
        {
            // Disallow closed paren immediately followed by a number
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

    private void backspaceEquationText()
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

    protected void OnButtonBackspaceClicked(object sender, System.EventArgs e)
    {
        backspaceEquationText();
    }

    protected void OnKeyReleaseEvent(object o, Gtk.KeyReleaseEventArgs args)
    {
        switch (args.Event.Key)
        {
            case Gdk.Key.plus:
                operatorButtonClicked(new Plus());
                break;
            case Gdk.Key.minus:
                operatorButtonClicked(new Minus());
                break;
            case Gdk.Key.slash:
                operatorButtonClicked(new ForwardSlash());
                break;
            case Gdk.Key.asterisk:
                operatorButtonClicked(new Asterisk());
                break;
            case Gdk.Key.caret:
                operatorButtonClicked(new Caret());
                break;
            case Gdk.Key.Key_0:
                numberButtonClicked(0);
                break;
            case Gdk.Key.Key_1:
                numberButtonClicked(1);
                break;
            case Gdk.Key.Key_2:
                numberButtonClicked(2);
                break;
            case Gdk.Key.Key_3:
                numberButtonClicked(3);
                break;
            case Gdk.Key.Key_4:
                numberButtonClicked(4);
                break;
            case Gdk.Key.Key_5:
                numberButtonClicked(5);
                break;
            case Gdk.Key.Key_6:
                numberButtonClicked(6);
                break;
            case Gdk.Key.Key_7:
                numberButtonClicked(7);
                break;
            case Gdk.Key.Key_8:
                numberButtonClicked(8);
                break;
            case Gdk.Key.Key_9:
                numberButtonClicked(9);
                break;
            case Gdk.Key.ISO_Enter:
            case Gdk.Key.Key_3270_Enter:
            case Gdk.Key.KP_Enter:
                solveEquation();
                break;
            case Gdk.Key.Escape:
                clearEquationText();
                break;
            case Gdk.Key.BackSpace:
                backspaceEquationText();
                break;
            case Gdk.Key.parenleft:
                openParenthesis();
                break;
            case Gdk.Key.parenright:
                closeParenthesis();
                break;
        }
    }

    protected void OnButtonExponentClicked(object sender, System.EventArgs e)
    {
        operatorButtonClicked(new Caret());
    }

    private void openParenthesis()
    {
        var sym = new OpenParenthesis();
        if (ERROR_MESSAGE.Equals(_equationText) ||
            "0".Equals(_equationText))
        {
            changeEquationText(sym.ToString());
            return;
        }
        var eq = new Equation(_equationText);
        if (!eq.HasFinalOperator())
        {
            // Disallow a number followed by an open parenthesis
            return;
        }
        changeEquationText(_equationText + sym.ToString());
    }

    protected void OnButtonOpenParenthesisClicked(object sender, System.EventArgs e)
    {
        openParenthesis();
    }

    private void closeParenthesis()
    {
        var sym = new ClosedParenthesis();
        if (ERROR_MESSAGE.Equals(_equationText))
        {
            return;
        }
        char lastChar = _equationText[_equationText.Length - 1];
        if (new OpenParenthesis().ToString().Equals(lastChar.ToString()))
        {
            // Disallow open paren immediately followed by a closed paren: ()
            return;
        }
        changeEquationText(_equationText + sym.ToString());
    }

    protected void OnButtonClosedParenthesisClicked(object sender, System.EventArgs e)
    {
        closeParenthesis();
    }
}
