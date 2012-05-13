//  
//  WordulaTranslator.cs
//  
//  Author:
//       Sarah Vessels <cheshire137@gmail.com>
// 
//  Copyright (c) 2012 Sarah Vessels
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Main
{
    public class WordulaTranslator
    {
        private readonly string _equation;

        public WordulaTranslator(string equation)
        {
            _equation = equation;
        }

        public string Translate() {
            var words = new List<string>();
            string toTranslate = _equation;
            while (toTranslate.Length > 0) {
                consumeDigit(toTranslate, out toTranslate, ref words);
                if (toTranslate.Length > 0) {
                    words.Add(charToWord(toTranslate.First()));
                    toTranslate = toTranslate.Substring(1);
                }
            }
            return string.Join(" ", words.ToArray());
        }

        private static bool consumeDigit(string input, out string remaining,
                                         ref List<string> words)
        {
            var digitRegex = new Regex(@"\d");
            IEnumerable<char> digits =
                input.TakeWhile(c => digitRegex.IsMatch(c.ToString()));
            string number = string.Join("",
                  digits.Select(c => c.ToString()).ToArray());
            if (number.Length == 1) {
                words.Add(charToWord(number.First()));
            } else {
                words.Add(numberToWord(number));
            }
            if (digits.Count() > 0) {
                remaining = input.Substring(digits.Count());
                return true;
            }
            remaining = input;
            return false;
        }

        // Thanks to http://www.eggheadcafe.com/community/csharp/2/10018714/convert-number--into-words.aspx
        private static string tensToWord(string numberStr) {
            int number = Convert.ToInt32(numberStr);
            switch (number) {
                case 10:
                    return "ten";
                case 11:
                    return "eleven";
                case 12:
                    return "twelve";
                case 13:
                    return "thirteen";
                case 14:
                    return "fourteen";
                case 15:
                    return "fifteen";
                case 16:
                    return "sixteen";
                case 17:
                    return "seventeen";
                case 18:
                    return "eighteen";
                case 19:
                    return "nineteen";
                case 20:
                    return "twenty";
                case 30:
                    return "thirty";
                case 40:
                    return "forty";
                case 50:
                    return "fifty";
                case 60:
                    return "sixty";
                case 70:
                    return "seventy";
                case 80:
                    return "eighty";
                case 90:
                    return "ninety";
            }
            if (number > 0) {
                return tensToWord(numberStr.First() + "0") + " "
                    + charToWord(numberStr.Substring(1).First());
            }
            return "";
        }

        // Thanks to http://www.eggheadcafe.com/community/csharp/2/10018714/convert-number--into-words.aspx
        private static string numberToWord(string numberStr) {
            Console.Out.WriteLine(numberStr);
            double dblValue = Convert.ToDouble(numberStr);
            int numDigits = numberStr.Length;
            if (0 == dblValue) {
                return "";
            }
            int pos = 0;
            string place = "";
            switch (numDigits) {
                case 1:
                    return charToWord(numberStr.First());
                case 2:
                    return tensToWord(numberStr);
                case 3:
                    pos = (numDigits % 3) + 1;
                    place = " hundred ";
                    break;
                case 4:
                case 5:
                case 6:
                    pos = (numDigits % 4) + 1;
                    place = " thousand ";
                    break;
                case 7:
                case 8:
                case 9:
                    pos = (numDigits % 7) + 1;
                    place = " million ";
                    break;
                case 10:
                    pos = (numDigits % 10) + 1;
                    place = " billion ";
                    break;
            }
            string word = numberToWord(numberStr.Substring(0, pos)) + place
                + numberToWord(numberStr.Substring(pos));
            if (numberStr.StartsWith("0")) {
                word = " and " + word.Trim();
            }
            if (word.Trim().Equals(place.Trim())) {
                word = "";
            }
            return word.Trim();
        }

        private static string charToWord(char c)
        {
            switch (c)
            {
                case '0':
                    return "zero";
                case '1':
                    return "one";
                case '2':
                    return "two";
                case '3':
                    return "three";
                case '4':
                    return "four";
                case '5':
                    return "five";
                case '6':
                    return "six";
                case '7':
                    return "seven";
                case '8':
                    return "eight";
                case '9':
                    return "nine";
                case ' ':
                case '\t':
                case '\r':
                case '\n':
                    return "";
                case '+':
                    return "plus";
                case '-':
                    return "minus";
                case '*':
                    return "times";
                case '/':
                    return "divided by";
                case '^':
                    return "to the";
                case '.':
                    return "point";
                case '(':
                    return "parenthesis";
                case ')':
                    return "close parenthesis";
                default:
                    return "";
            }
        }
    }
}

