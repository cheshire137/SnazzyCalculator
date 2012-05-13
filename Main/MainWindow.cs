using System;
using Gtk;
using Calculator;
using System.Linq;

namespace Main
{
    public partial class MainWindow : Gtk.Window
    {
        private static readonly Gdk.Color _numberButtonBg =
        new Gdk.Color(242, 204, 133);
        private static readonly Gdk.Color _numberButtonHoverBg =
        new Gdk.Color(253, 221, 160);
        private static readonly Gdk.Color _opButtonBg =
        new Gdk.Color(166, 123, 123);
        private static readonly Gdk.Color _opButtonHoverBg =
        new Gdk.Color(181, 146, 146);
        private static readonly Gdk.Color _miscButtonBg =
        new Gdk.Color(191, 160, 142);
        private static readonly Gdk.Color _miscButtonHoverBg =
        new Gdk.Color(206, 180, 165);
        private static readonly Gdk.Color _solveButtonBg =
        new Gdk.Color(170, 242, 133);
        private static readonly Gdk.Color _solveButtonHoverBg =
        new Gdk.Color(188, 255, 153);
        private static readonly Gdk.Color _undoButtonBg =
        new Gdk.Color(242, 133, 133);
        private static readonly Gdk.Color _undoButtonHoverBg =
        new Gdk.Color(255, 155, 155);
        private static Pango.FontDescription _buttonFont = null;
        private const string ERROR_MESSAGE = "ERROR";
        private const string FONT_DESCRIPTION = "American Typewriter Regular 26";
        private Pango.Layout _equationLayout = null;
        private string _equationText = "0";
        private string _equationDisplay = "0";
        private bool _wordulatorMode;

        public MainWindow() : base (Gtk.WindowType.Toplevel)
        {
            _wordulatorMode = false;
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

        private void divideButtonClicked()
        {
            operatorButtonClicked(new ForwardSlash());
        }

        protected void OnButtonDivideClicked(object sender, System.EventArgs e)
        {
            divideButtonClicked();
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
            minusButtonClicked();
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
            try {
                Formula formula = SimpleFormulaParser.ParseFormula(
                    _equationText
                );
                //Console.WriteLine(formula.Inspect());
                changeEquationText("" + formula.Solve());
            } catch (ParserException ex)
            {
                changeEquationText(ERROR_MESSAGE);
                Console.WriteLine(ex.Message);
            } catch (DivideByZeroException)
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

        private void minusButtonClicked()
        {
            Symbol minusSym = new Minus();
            if ("0".Equals(_equationText))
            {
                changeEquationText(
                minusSym.ToString()
            );
                return;
            }
            operatorButtonClicked(minusSym);
        }
 
        private void operatorButtonClicked(Symbol op)
        {
            if (ERROR_MESSAGE.Equals(_equationText))
            {
                changeEquationText("0" + op.ToString());
                return;
            }
            var eq = new Equation(_equationText);
            if (eq.HasFinalOperator())
            {
                changeEquationText(eq.ReplaceFinalOperator(op));
            } else
            {
                changeEquationText(_equationText
                               + op.ToString());
            }
        }

        private void numberButtonClicked(int number)
        {
            if ("0".Equals(_equationText) ||
             ERROR_MESSAGE.Equals(_equationText))
            {
                changeEquationText("" + number);
                return;
            }
            // Disallow closed paren immediately followed by a number
            char lastChar = _equationText[_equationText.Length - 1];
            if (new ClosedParenthesis().ToString().Equals(lastChar.ToString()))
            {
                return;
            }
            changeEquationText(_equationText + number);
        }

        protected void OnEquationAreaExposeEvent(object o, Gtk.ExposeEventArgs args)
        {
            changeEquationText(_equationText);
        }

        private string translateEquation(string value)
        {
            if (_wordulatorMode) {
                var translator = new WordulaTranslator(value);
                return translator.Translate();
            }
            return value;
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
            _equationDisplay = translateEquation(value);
            _equationText = value;
            _equationLayout.SetMarkup(_equationDisplay);
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

        private void plusButtonClicked()
        {
            operatorButtonClicked(new Plus());
        }

        private void timesButtonClicked()
        {
            operatorButtonClicked(new Asterisk());
        }

        protected void OnKeyReleaseEvent(object o, Gtk.KeyReleaseEventArgs args)
        {
            switch (args.Event.Key)
            {
                case Gdk.Key.plus:
                    plusButtonClicked();
                    break;
                case Gdk.Key.minus:
                    minusButtonClicked();
                    break;
                case Gdk.Key.slash:
                    divideButtonClicked();
                    break;
                case Gdk.Key.asterisk:
                    timesButtonClicked();
                    break;
                case Gdk.Key.caret:
                case Gdk.Key.ccircumflex:
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
                case Gdk.Key.equal:
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
                case Gdk.Key.period:
                    operatorButtonClicked(new Period());
                    break;
            }
        }

        protected void OnButtonExponentClicked(object sender, System.EventArgs e)
        {
            operatorButtonClicked(new Caret());
        }

        private void openParenthesis()
        {
            Symbol sym = new OpenParenthesis();
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

        private bool isOpenParenLast()
        {
            char lastChar = _equationText[_equationText.Length - 1];
            if (new OpenParenthesis().ToString().Equals(lastChar.ToString()))
            {
                return true;
            }
            return false;
        }

        private void closeParenthesis()
        {
            if (ERROR_MESSAGE.Equals(_equationText))
            {
                return;
            }
            if (isOpenParenLast())
            {
                // Disallow open paren immediately followed by a closed paren: ()
                return;
            }
            Symbol sym = new ClosedParenthesis();
            changeEquationText(_equationText + sym.ToString());
        }

        protected void OnButtonClosedParenthesisClicked(object sender, System.EventArgs e)
        {
            closeParenthesis();
        }

        private static Pango.FontDescription getButtonFont()
        {
            if (null == _buttonFont)
            {
                _buttonFont = new Pango.FontDescription();
                _buttonFont.Family = "Arial Rounded MT Bold";
                _buttonFont.AbsoluteSize = 16 * Pango.Scale.PangoScale;
            }
            return _buttonFont;
        }

        private static void styleNumberButton(Gtk.Button button)
        {
            button.ModifyBg(StateType.Normal, _numberButtonBg);
            button.ModifyBg(StateType.Prelight, _numberButtonHoverBg);
            button.Child.ModifyFont(getButtonFont());
        }

        protected void OnButton7ExposeEvent(object o, Gtk.ExposeEventArgs args)
        {
            styleNumberButton(button7);
        }

        protected void OnButton8ExposeEvent(object o, Gtk.ExposeEventArgs args)
        {
            styleNumberButton(button8);
        }

        protected void OnButton9ExposeEvent(object o, Gtk.ExposeEventArgs args)
        {
            styleNumberButton(button9);
        }

        protected void OnButton4ExposeEvent(object o, Gtk.ExposeEventArgs args)
        {
            styleNumberButton(button4);
        }

        protected void OnButton5ExposeEvent(object o, Gtk.ExposeEventArgs args)
        {
            styleNumberButton(button5);
        }

        protected void OnButton6ExposeEvent(object o, Gtk.ExposeEventArgs args)
        {
            styleNumberButton(button6);
        }

        protected void OnButton1ExposeEvent(object o, Gtk.ExposeEventArgs args)
        {
            styleNumberButton(button1);
        }

        protected void OnButton2ExposeEvent(object o, Gtk.ExposeEventArgs args)
        {
            styleNumberButton(button2);
        }

        protected void OnButton3ExposeEvent(object o, Gtk.ExposeEventArgs args)
        {
            styleNumberButton(button3);
        }

        protected void OnButton0ExposeEvent(object o, Gtk.ExposeEventArgs args)
        {
            styleNumberButton(button0);
        }

        private static void styleOperatorButton(Gtk.Button button)
        {
            button.ModifyBg(StateType.Normal, _opButtonBg);
            button.ModifyBg(StateType.Prelight, _opButtonHoverBg);
            button.Child.ModifyFont(getButtonFont());
        }

        protected void OnButtonDivideExposeEvent(object o, Gtk.ExposeEventArgs args)
        {
            styleOperatorButton(buttonDivide);
        }

        protected void OnButtonMultiplyExposeEvent(object o, Gtk.ExposeEventArgs args)
        {
            styleOperatorButton(buttonMultiply);
        }

        protected void OnButtonSubtractExposeEvent(object o, Gtk.ExposeEventArgs args)
        {
            styleOperatorButton(buttonSubtract);
        }

        protected void OnButtonAddExposeEvent(object o, Gtk.ExposeEventArgs args)
        {
            styleOperatorButton(buttonAdd);
        }

        protected void OnButtonExponentExposeEvent(object o, Gtk.ExposeEventArgs args)
        {
            styleOperatorButton(buttonExponent);
        }

        private static void styleMiscButton(Gtk.Button button)
        {
            button.ModifyBg(StateType.Normal, _miscButtonBg);
            button.ModifyBg(StateType.Prelight, _miscButtonHoverBg);
            button.Child.ModifyFont(getButtonFont());
        }

        protected void OnButtonOpenParenthesisExposeEvent(object o, Gtk.ExposeEventArgs args)
        {
            styleMiscButton(buttonOpenParenthesis);
        }

        protected void OnButtonClosedParenthesisExposeEvent(object o, Gtk.ExposeEventArgs args)
        {
            styleMiscButton(buttonClosedParenthesis);
        }

        protected void OnButtonDotExposeEvent(object o, Gtk.ExposeEventArgs args)
        {
            styleMiscButton(buttonDot);
        }

        private static void styleSolveButton(Gtk.Button button)
        {
            button.ModifyBg(StateType.Normal, _solveButtonBg);
            button.ModifyBg(StateType.Prelight, _solveButtonHoverBg);
            button.Child.ModifyFont(getButtonFont());
        }

        protected void OnButtonEqualsExposeEvent(object o, Gtk.ExposeEventArgs args)
        {
            styleSolveButton(buttonEquals);
        }

        private static void styleUndoButton(Gtk.Button button)
        {
            button.ModifyBg(StateType.Normal, _undoButtonBg);
            button.ModifyBg(StateType.Prelight, _undoButtonHoverBg);
            button.Child.ModifyFont(getButtonFont());
        }

        protected void OnButtonClearExposeEvent(object o, Gtk.ExposeEventArgs args)
        {
            styleUndoButton(buttonClear);
        }

        protected void OnButtonBackspaceExposeEvent(object o, Gtk.ExposeEventArgs args)
        {
            styleUndoButton(buttonBackspace);
        }

        protected void OnWordModeCheckboxToggled(object sender, System.EventArgs e)
        {
            _wordulatorMode = !_wordulatorMode;
            // Force it to update the translation
            changeEquationText(_equationText);
        }
    }
}