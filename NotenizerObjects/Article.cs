using nsInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nsEnums;

namespace nsNotenizerObjects
{
    public class Article : IPersistable
    {
        private String _id;
        private DateTime _createdAt;
        private DateTime _updatedAt;
        private CreatedBy _createdBy;
        private String _text;

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
                return this._createdBy;
            }

            set
            {
                this._createdBy = value;
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

        public String Value
        {
            get
            {
                return this._text;
            }
        }
    }
}
