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
        #region Variables

        private SentencesTerminators _sentencesTerminators;

        #endregion Variables

        #region Constructors

        public NotenizerNoteRule(String id, String structureId, DateTime createdAt, DateTime updatedAt, SentencesTerminators sentencesTerminators)
            : base(id, structureId, createdAt, updatedAt)
        {
            this._sentencesTerminators = sentencesTerminators;
        }

        public NotenizerNoteRule() : base()
        {
            _sentencesTerminators = null;
        }

        #endregion Constuctors

        #region Properties

        public SentencesTerminators SentencesTerminators
        {
            get { return _sentencesTerminators; }
            set { this._sentencesTerminators = value; }
        }

        #endregion Properties

        #region Methods

        #endregion Methods
    }
}
