using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace nsExceptions
{
    /// <summary>
    /// Handles exception inside tasks.
    /// </summary>
    public static class TaskExceptionHandler
    {
        public static void Handle(Task task)
        {
            AggregateException exception = task.Exception;

            if (exception != null)
                Console.WriteLine(exception);

            MessageBox.Show(
                String.Format("Unexpected error has occured.{0}{0}{1}", Environment.NewLine, exception.Message),
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
