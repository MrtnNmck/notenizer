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
        private SentencesTerminators _sentencesTerminators;

        public NotenizerNoteRule(String id, NotenizerDependencies dependencies, SentencesTerminators sentencesEnds, CreatedBy createdBy)
            : base(id, dependencies, createdBy)
        {
            _sentencesTerminators = sentencesEnds;
        }

        public NotenizerNoteRule(String id, String structureId, DateTime createdAt, DateTime updatedAt, SentencesTerminators sentencesTerminators)
            : base(id, structureId, createdAt, updatedAt)
        {
            this._sentencesTerminators = sentencesTerminators;
        }

        public NotenizerNoteRule(CreatedBy createdBy) : base(createdBy)
        {
            _sentencesTerminators = null;
        }

        public SentencesTerminators SentencesTerminators
        {
            get { return _sentencesTerminators; }
            set { this._sentencesTerminators = value; }
        }
    }
}
