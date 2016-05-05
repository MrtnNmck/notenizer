using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace nsExtensions
{
    /// <summary>
    /// Extension for threads.
    /// </summary>
    public static class ThreadExtensions
    {
        #region Methods

        /// <summary>
        /// Performs action safaly.
        /// Handles "Control accessed from other thread that it was created on" exception.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="action"></param>
        public static void PerformSafely(this Control target, Action action)
        {
            if (target.InvokeRequired)
            {
                target.Invoke(action);
            }
            else
            {
                action();
            }
        }

        /// <summary>
        /// Performs action safaly.
        /// Handles "Control accessed from other thread that it was created on" exception.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="action"></param>
        /// <param name="parameter"></param>
        public static void PerformSafely<T>(this Control target, Action<T> action, T parameter)
        {
            if (target.InvokeRequired)
            {
                target.Invoke(action, parameter);
            }
            else
            {
                action(parameter);
            }
        }

        /// <summary>
        /// Performs action safaly.
        /// Handles "Control accessed from other thread that it was created on" exception.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="target"></param>
        /// <param name="action"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        public static void PerformSafely<T1, T2>(this Control target, Action<T1, T2> action, T1 p1, T2 p2)
        {
            if (target.InvokeRequired)
            {
                target.Invoke(action, p1, p2);
            }
            else
            {
                action(p1, p2);
            }
        }

        #endregion Methods
    }
}