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
        private int _sentenceTerminator;

        public NotenizerAndRule(String id, NotenizerDependencies dependencies, CreatedBy createdBy, int setsPosition, int sentenceEnd)
            : base(id, dependencies)
        {
            _setsPosition = setsPosition;
            _sentenceTerminator = sentenceEnd;
        }

        public NotenizerAndRule(NotenizerDependencies dependencies, int setsPosition, int sentenceEnd)
            : base(String.Empty, dependencies)
        {
            _setsPosition = setsPosition;
            _sentenceTerminator = sentenceEnd;
        }

        public NotenizerAndRule(String id, String structureId, DateTime createdAt, DateTime updatedAt, int setsPosition, int sentenceTerminator)
            : base(id, structureId, createdAt, updatedAt)
        {
            this._setsPosition = setsPosition;
            this._sentenceTerminator = sentenceTerminator;
        }

        public int SetsPosition
        {
            get { return _setsPosition; }
            set { _setsPosition = value; }
        }

        public int SentenceTerminator
        {
            get { return _sentenceTerminator; }
            set { _sentenceTerminator = value; }
        }
    }
}
