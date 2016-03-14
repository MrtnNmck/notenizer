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
        public NotenizerParser()
        {
        }

        public virtual void Parse(NotenizerSentence sentence)
        {
        }

        public virtual bool IsParsableSentence(NotenizerSentence sentence)
        {
            return false;
        }
    }
}
