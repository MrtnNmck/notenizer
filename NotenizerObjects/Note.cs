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
        private String _text;
        private DateTime _createdAt;
        private DateTime _updatedAt;
        private CreatedBy _createdBy;
        private String _andRuleId;
        private String _ruleId;
        private String _sentenceId;

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
            _text = persistedNote;
            _createdAt = persistedCreatedAt;
            _updatedAt = persistedUpdatedAt;
            _createdBy = persistedCreatedBy;
            _andRuleId = andParserRuleRefId;
        }

        public Note(String text)
        {
            this._text = text;
        }

        public String ID
        {
            get { return _id; }
            set { this._id = value; }
        }

        public String Text
        {
            get { return _text; }
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

        public String AndRuleID
        {
            get { return _andRuleId; }
            set { this._andRuleId = value; }
        }

        public String SentenceID
        {
            get { return this._sentenceId; }
            set { this._andRuleId = value; }
        }

        public String RuleID
        {
            get { return this._ruleId; }
            set { this._ruleId = value; }
        }
    }
}
