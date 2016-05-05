using nsInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nsEnums;

namespace nsNotenizerObjects
{
    /// <summary>
    /// persistable sentence.
    /// </summary>
    public class Sentence : IPersistable
    {
        #region Variables

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

        #endregion Variables

        #region Constructors

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

        #endregion Constuctors

        #region Properties

        /// <summary>
        /// Created at timestamp.
        /// </summary>
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

        /// <summary>
        /// ID of document in database.
        /// </summary>
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

        /// <summary>
        /// Updated at timestamp.
        /// </summary>
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

        /// <summary>
        /// Text of sentence.
        /// </summary>
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

        /// <summary>
        /// ID of struncture document.
        /// </summary>
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

        /// <summary>
        /// ID of rule document.
        /// </summary>
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

        /// <summary>
        /// ID of and-rule document.
        /// </summary>
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

        /// <summary>
        /// ID of article document.
        /// </summary>
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

        /// <summary>
        /// ID of note document.
        /// </summary>
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

        /// <summary>
        /// Article.
        /// </summary>
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

        #endregion Properties

        #region Methods

        #endregion Methods
    }
}
