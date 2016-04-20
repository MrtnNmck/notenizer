using nsEnums;
using nsInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsNotenizerObjects
{
    public class Note : IPersistable
    {
        private String _id;
        private String _note;
        private String _originalSentence;
        private DateTime _createdAt;
        private DateTime _updatedAt;
        private CreatedBy _createdBy;
        private String _andParserRuleRefId;

        public Note(
            String id,
            String persistedOriginalSentence,
            String persistedNote,
            DateTime persistedCreatedAt,
            DateTime persistedUpdatedAt,
            CreatedBy persistedCreatedBy,
            String andParserRuleRefId)
        {
            _id = id;
            _note = persistedNote;
            _originalSentence = persistedOriginalSentence;
            _createdAt = persistedCreatedAt;
            _updatedAt = persistedUpdatedAt;
            _createdBy = persistedCreatedBy;
            _andParserRuleRefId = andParserRuleRefId;
        }

        public String ID
        {
            get { return _id; }
        }

        public String Value
        {
            get { return _note; }
        }

        public String OriginalSentence
        {
            get { return _originalSentence; }
        }

        public DateTime CreatedAt
        {
            get { return _createdAt; }
            set { this._createdAt = value; }
        }

        public DateTime UpdatedAt
        {
            get { return _updatedAt; }
            set { this._updatedAt = value; }
        }

        public CreatedBy CreatedBy
        {
            get { return _createdBy; }
            set { this._createdBy = value; }
        }

        public String AndParserRuleRefId
        {
            get { return _andParserRuleRefId; }
        }
    }
}
