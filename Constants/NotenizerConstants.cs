using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsConstants
{
    public class NotenizerConstants
    {
        public const String WordDelimeter = " ";
        public const String SentenceFinisher = ".";
        public const String SentenceDelimeter = SentenceFinisher + WordDelimeter;
        public const int UninitializedDependencyPositionValue = Int32.MinValue;
    }
}
