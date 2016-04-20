using MongoDB.Bson;
using nsEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsNotenizerObjects
{
    public class NotenizerNoteRule : NotenizerRule
    {
        private List<int> _sentencesEnds;

        public NotenizerNoteRule(String id, NotenizerDependencies dependencies, List<int> sentencesEnds, CreatedBy createdBy)
            : base(id, dependencies, createdBy)
        {
            _sentencesEnds = sentencesEnds;
        }

        public NotenizerNoteRule(CreatedBy createdBy) : base(createdBy)
        {
            _sentencesEnds = null;
        }

        public List<int> SentencesEnds
        {
            get { return _sentencesEnds; }
            set { this._sentencesEnds = value; }
        }
    }
}
