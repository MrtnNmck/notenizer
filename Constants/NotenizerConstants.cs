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
        public const String SentenceTerminator = ".";
        public const String SentenceDelimeter = SentenceTerminator + WordDelimeter;
        public const int UninitializedDependencyPositionValue = Int32.MinValue;
        public const double MaxMatchValue = 100.0;
        public const double MinMatchValue = 0.0;
        public const String SimpleWikiUrl = "https://simple.wikipedia.org/wiki/";
    }
}
