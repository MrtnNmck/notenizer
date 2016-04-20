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
        private String _value;

        public Article(
            String id,
            DateTime createdAt,
            DateTime updatedAt,
            CreatedBy createdBy,
            String value)
        {
            this._id = id;
            this._createdAt = createdAt;
            this._updatedAt = updatedAt;
            this._createdBy = createdBy;
            this._value = value;
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
                this.ID = value;
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
                return this._value;
            }
        }
    }
}
