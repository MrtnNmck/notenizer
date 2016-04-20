using nsInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nsEnums;

namespace nsNotenizerObjects
{
    public class Structure : IPersistable
    {
        private DateTime _createdAt;
        private DateTime _updatedAt;
        private String _id;

        public Structure(DateTime createdAt, DateTime updatedAt)
        {
            _createdAt = createdAt;
            _updatedAt = updatedAt;
        }

        public Structure(String id, DateTime createdAt, DateTime updatedAt)
        {
            _id = id;
            _createdAt = createdAt;
            _updatedAt = updatedAt;
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
    }
}
