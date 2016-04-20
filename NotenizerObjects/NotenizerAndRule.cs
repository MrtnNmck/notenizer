using nsEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsNotenizerObjects
{
    public class NotenizerAndRule : NotenizerRule
    {
        private int _setsPosition;
        private int _sentenceEnd;

        public NotenizerAndRule(String id, NotenizerDependencies dependencies, CreatedBy createdBy, int setsPosition, int sentenceEnd)
            : base(id, dependencies, createdBy)
        {
            _setsPosition = setsPosition;
            _sentenceEnd = sentenceEnd;
        }

        public NotenizerAndRule(NotenizerDependencies dependencies, CreatedBy createdBy, int setsPosition, int sentenceEnd)
            : base(String.Empty, dependencies, createdBy)
        {
            _setsPosition = setsPosition;
            _sentenceEnd = sentenceEnd;
        }

        public int SetsPosition
        {
            get { return _setsPosition; }
            set { _setsPosition = value; }
        }

        public int SentenceEnd
        {
            get { return _sentenceEnd; }
            set { _sentenceEnd = value; }
        }
    }
}
