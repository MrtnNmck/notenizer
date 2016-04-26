﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsNotenizerObjects
{
    public class SentencesTerminators : List<int>
    {
        public SentencesTerminators()
        {
        }

        public SentencesTerminators(List<int> sentenceTerminators)
        {
            this.AddRange(sentenceTerminators);
        }
    }
}