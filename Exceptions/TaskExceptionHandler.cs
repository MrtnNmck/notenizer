using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsExceptions
{
    public static class TaskExceptionHandler
    {
        public static void Handle(Task task)
        {
            AggregateException exception = task.Exception;

            if (exception != null)
                Console.WriteLine(exception);
        }
    }
}
