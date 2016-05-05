using nsNotenizerObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsParsers
{
    /// <summary>
    /// Universal parser.
    /// </summary>
    public class NotenizerParser
    {
        #region Variables

        #endregion Variables

        #region Constructors

        public NotenizerParser()
        {
        }

        #endregion Constuctors

        #region Properties

        #endregion Properties

        #region Methods

        /// <summary>
        /// Parses sentence.
        /// </summary>
        /// <param name="sentence"></param>
        /// <returns></returns>
        public virtual NotenizerNote Parse(NotenizerSentence sentence)
        {
            return null;
        }

        /// <summary>
        /// Checks if sentence is parsable.
        /// </summary>
        /// <param name="sentence"></param>
        /// <returns></returns>
        public virtual bool IsParsableSentence(NotenizerSentence sentence)
        {
            return false;
        }

        #endregion Methods
    }
}
