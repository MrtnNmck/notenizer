using nsNotenizerObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsParsers
{
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

        public virtual NotenizerNote Parse(NotenizerSentence sentence)
        {
            return null;
        }

        public virtual bool IsParsableSentence(NotenizerSentence sentence)
        {
            return false;
        }

        #endregion Methods
    }
}
