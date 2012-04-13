using System;
using Gtk;
using Calculator;

public partial class MainWindow : Gtk.Window
{
	private const string ERROR_MESSAGE = "ERROR";
	
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
		throw new System.NotImplementedException();
	}

	protected void OnButtonAddClicked(object sender, System.EventArgs e)
	{
		operatorButtonClicked('+');
	}

	protected void OnButtonEqualsClicked(object sender, System.EventArgs e)
	{
		var eq = new Equation(equationTextView.Buffer.Text);
		if (eq.IsValid())
		{
			equationTextView.Buffer.Text = "" + eq.Solve();
		}
		else
		{
			equationTextView.Buffer.Text = ERROR_MESSAGE;
		}
	}

	protected void OnButtonClearClicked(object sender, System.EventArgs e)
	{
		equationTextView.Buffer.Text = "0";
	}
	
	private void operatorButtonClicked(char op)
	{
		if (ERROR_MESSAGE.Equals(equationTextView.Buffer.Text))
		{
			equationTextView.Buffer.Text = "0" + op;
			return;
		}
		var eq = new Equation(equationTextView.Buffer.Text);
		if (eq.HasFinalOperator()) {
			equationTextView.Buffer.Text = eq.ReplaceFinalOperator(op);
		}
		else
		{
			equationTextView.Buffer.Text += op;
		}
	}
	
	private void numberButtonClicked(int number)
	{
		string curEquation = equationTextView.Buffer.Text;
		if ("0".Equals(curEquation) ||
		    	ERROR_MESSAGE.Equals(curEquation)) {
			equationTextView.Buffer.Text = "" + number;
			return;
		}
		equationTextView.Buffer.Text += number;
	}
}
