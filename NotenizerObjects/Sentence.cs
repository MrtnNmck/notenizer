using nsInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nsEnums;

namespace nsNotenizerObjects
{
    public class Sentence : IPersistable
    {
        private DateTime _createdAt;
        private DateTime _updatedAt;
        private String _id;
        private String _text;
        private String _articleID;
        private String _structureID;
        private String _ruleID;
        private String _andRuleID;
        private String _noteID;
        private Article _article;

        public Sentence(String text, Article article)
        {
            this._article = article;
            this._text = text;
            this._createdAt = DateTime.Now;
            this._updatedAt = DateTime.Now;
        }

        public Sentence(
            String id,
            String text,
            String articleID,
            String structureID,
            String ruleID,
            String andRuleID,
            String noteId,
            DateTime createdAt,
            DateTime updatedAt)
        {
            this._id = id;
            this._text = text;
            this._articleID = articleID;
            this._structureID = structureID;
            this._ruleID = ruleID;
            this._andRuleID = andRuleID;
            this._noteID = noteId;
            this._createdAt = createdAt;
            this._updatedAt = updatedAt;
        }

        public DateTime CreatedAt
        {
            get
            {
                return this._createdAt;
            }

            set
            {
                this._createdAt = value;
            }
        }

        public CreatedBy CreatedBy
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public String ID
        {
            get
            {
                return this._id;
            }

            set
            {
                this._id = value;
            }
        }

        public DateTime UpdatedAt
        {
            get
            {
                return this._updatedAt;
            }

            set
            {
                this._updatedAt = value;
            }
        }

        public String Text
        {
            get
            {
                return this._text;
            }

            set
            {
                this._text = value;
            }
        }

        public String StructureID
        {
            get
            {
                return _structureID;
            }

            set
            {
                _structureID = value;
            }
        }

        public String RuleID
        {
            get
            {
                return _ruleID;
            }

            set
            {
                _ruleID = value;
            }
        }

        public String AndRuleID
        {
            get
            {
                return _andRuleID;
            }

            set
            {
                _andRuleID = value;
            }
        }

        public String ArticleID
        {
            get
            {
                return _articleID;
            }

            set
            {
                _articleID = value;
            }
        }

        public String NoteID
        {
            get
            {
                return _noteID;
            }

            set
            {
                _noteID = value;
            }
        }

        public Article Article
        {
            get
            {
                return _article;
            }

            set
            {
                _article = value;
            }
        }
    }
}
