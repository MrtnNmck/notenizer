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

        public Note(
            String id,
            String text,
            String ruleID,
            String andRuleID,
            DateTime createdAt,
            DateTime updatedAt)
        {
            this._id = id;
            this._text = text;
            this._ruleId = ruleID;
            this._andRuleId = andRuleID;
            this._createdAt = createdAt;
            this._updatedAt = updatedAt;
        }

        public Note(String text)
        {
            this._text = text;
            this._createdAt = DateTime.Now;
            this._updatedAt = DateTime.Now;
        }

        public String ID
        {
            get { return _id; }
            set { this._id = value; }
        }

        public String Text
        {
            get { return _text; }
            set { this._text = value; }
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
        
        public String RuleID
        {
            get { return this._ruleId; }
            set { this._ruleId = value; }
        }
    }
}
