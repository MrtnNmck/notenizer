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
    /// <summary>
    /// Extension methods on C# objects.
    /// </summary>
    public static class Extensions
    {
        #region Methods

        /// <summary>
        /// COnverts Java's ArrayList to C# List.
        /// </summary>
        /// <typeparam name="T">Type of items in list</typeparam>
        /// <param name="javaArrayList">Java's ArrayList to convert</param>
        /// <returns></returns>
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

        /// <summary>
        /// Preppends text to string.
        /// </summary>
        /// <param name="source">Source string</param>
        /// <param name="preppendingText">String to preppend</param>
        /// <returns>Newly created string</returns>
		public static String Preppend(this String source, String preppendingText)
		{
            if (source.IsNullOrEmpty())
                return preppendingText;

			return source.Preppend(preppendingText, true);
		}

        /// <summary>
        /// Preppends text to string.
        /// </summary>
        /// <param name="source">Source string</param>
        /// <param name="preppendingText">String to preppend</param>
        /// <param name="bSeparate">Flag indicating if strings should be separated</param>
        /// <returns>Newly created string</returns>
		public static String Preppend(this String source, String preppendingText, bool bSeparate)
		{
			return source.Preppend(preppendingText, bSeparate ? NotenizerConstants.WordDelimeter : String.Empty);
		}

        /// <summary>
        /// Preppends text to string.
        /// </summary>
        /// <param name="source">Source string</param>
        /// <param name="preppendingText">String to preppend</param>
        /// <param name="separator">Separator to separate strings</param>
        /// <returns>Newly created string</returns>
		public static String Preppend(this String source, String preppendingText, String separator)
		{
			return source = preppendingText + separator + source;
		}

        /// <summary>
        /// Checks if string is null or empty.
        /// </summary>
        /// <param name="source">String to check</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this String source)
        {
            return String.IsNullOrEmpty(source);
        }

        /// <summary>
        /// Cpaitalizes sentence. Changed first letter to capital.
        /// </summary>
        /// <param name="sentence">Sentence to capitalize</param>
        /// <returns></returns>
        public static String CapitalizeSentence(this String sentence)
        {
            if (sentence == String.Empty)
                return String.Empty;

            return sentence.First().ToString().ToUpper() + sentence.Substring(1);
        }

        /// <summary>
        /// Terminates sentence. Adds sentence terminator to the end.
        /// </summary>
        /// <param name="sentence"></param>
        /// <param name="terminator"></param>
        /// <returns></returns>
        public static String TerminateSentence(this String sentence, String terminator)
        {
            if (sentence.EndsWith(terminator))
                return sentence;

            return sentence + terminator;
        }

        /// <summary>
        /// Converts object to integer.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static int ToInt(this Object source)
        {
            int i;

            if (Int32.TryParse(source.ToString(), out i))
                return i;
            else
                throw new Exception(source.ToString() + " is not an integer.");
        }

        /// <summary>
        /// Converts int to enum.
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="value">Integer value of enum</param>
        /// <returns></returns>
        public static T ToEnum<T>(this int value) where T : struct, IConvertible, IComparable, IFormattable
        {
            if (Enum.IsDefined(typeof(T), value))
            {
                return (T)Enum.Parse(typeof(T), value.ToString());
            }
            else
                throw new Exception(value + " is not a value in enum!");
        }

        /// <summary>
        /// Sets tooltip to control.
        /// </summary>
        /// <param name="c">Control to set tooltip to</param>
        /// <param name="toolTipText">Tooltip text</param>
        public static void SetToolTip(this Control c, String toolTipText)
        {
            ToolTip tooltip = new ToolTip();
            tooltip.IsBalloon = true;
            tooltip.InitialDelay = 0;
            tooltip.ShowAlways = true;
            tooltip.SetToolTip(c, toolTipText);
        }


        /// <summary>
        /// Normalizes whitespaces in string.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
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

        #endregion Methods
    }
}
