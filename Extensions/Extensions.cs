using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using java.util;
using java.io;
using edu.stanford.nlp.trees;
using edu.stanford.nlp.ling;
using nsConstants;
using System.Windows.Forms;

namespace nsExtensions
{
    public static class Extensions
    {
		public static List<T> ToList<T>(this java.util.ArrayList javaArrayList)
		{
			List<T> list = new List<T>();

			foreach (var item in javaArrayList)
			{
				if (item.GetType() != typeof(T))
					throw new Exception("Collection containes object of other type than " + typeof(T));

				list.Add((T)item);
			}

			return list;
		}

		public static String Preppend(this String source, String preppendingText)
		{
            if (source.IsNullOrEmpty())
                return preppendingText;

			return source.Preppend(preppendingText, true);
		}

		public static String Preppend(this String source, String preppendingText, bool bSeparate)
		{
			return source.Preppend(preppendingText, bSeparate ? NotenizerConstants.WordDelimeter : String.Empty);
		}

		public static String Preppend(this String source, String preppendingText, String separator)
		{
			return source = preppendingText + separator + source;
		}

        public static bool IsNullOrEmpty(this String source)
        {
            return String.IsNullOrEmpty(source);
        }

        public static String CapitalizeSentence(this String sentence)
        {
            if (sentence == String.Empty)
                return String.Empty;

            return sentence.First().ToString().ToUpper() + sentence.Substring(1);
        }

        public static String TerminateSentence(this String sentence, String terminator)
        {
            if (sentence.EndsWith(terminator))
                return sentence;

            return sentence + terminator;
        }

        public static int ToInt(this Object source)
        {
            int i;

            if (Int32.TryParse(source.ToString(), out i))
                return i;
            else
                throw new Exception(source.ToString() + " is not an integer.");
        }

        public static T ToEnum<T>(this int value) where T : struct, IConvertible, IComparable, IFormattable
        {
            if (Enum.IsDefined(typeof(T), value))
            {
                return (T)Enum.Parse(typeof(T), value.ToString());
            }
            else
                throw new Exception(value + " is not a value in enum!");
        }

        public static void SetToolTip(this Control c, String toolTipText)
        {
            ToolTip tooltip = new ToolTip();
            tooltip.IsBalloon = true;
            tooltip.InitialDelay = 0;
            tooltip.ShowAlways = true;
            tooltip.SetToolTip(c, toolTipText);
        }

        public static string NormalizeWhiteSpaces(this String source)
        {
            StringBuilder sb = new StringBuilder();
            bool isPrevWhiteWhiteSpace = false;

            foreach (Char c in source)
            {
                if (Char.IsWhiteSpace(c))
                {
                    if (isPrevWhiteWhiteSpace)
                        continue;
                    else
                        isPrevWhiteWhiteSpace = true;
                }
                else
                    isPrevWhiteWhiteSpace = false;

                // remove all whitespaces before sentence terminator
                if (GrammaticalConstants.SentenceTerminators.Contains(c) && isPrevWhiteWhiteSpace)
                {
                    sb.Remove(sb.Length - 1, 1);
                    isPrevWhiteWhiteSpace = false;
                }

                sb.Append(c);
            }

            return sb.ToString();
        }
    }
}
