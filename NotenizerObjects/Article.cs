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
    /// Processed Article.
    /// </summary>
    public class Article : IPersistable
    {
        #region Variables

        private String _id;
        private DateTime _createdAt;
        private DateTime _updatedAt;
        private String _text;

        #endregion Variables

        #region Constructors

        public Article(
            String id,
            DateTime createdAt,
            DateTime updatedAt,
            String value)
        {
            this._id = id;
            this._createdAt = createdAt;
            this._updatedAt = updatedAt;
            this._text = value;
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
        /// Updated at timestamp
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
        /// Text.
        /// </summary>
        public String Text
        {
            get
            {
                return this._text;
            }
        }

        #endregion Properties

        #region Methods

        #endregion Methods
    }
}
