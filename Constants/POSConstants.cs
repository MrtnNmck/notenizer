using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsConstants
{
    /// <summary>
    /// Part of speech constants.
    /// </summary>
    public class POSConstants
    {
        public static readonly String[] NounLikePOS = { "NN", "NNP", "JJR", "JJ", "CD" };
        public static readonly String[] VerbLikePOS = { "VB", "VBN", "VBG", "VBD", "VBP", "VBZ" };
        public static readonly String[] ConjustionPOS = { "NN", "NNP", "JJ", "JJR" };
    }
}
