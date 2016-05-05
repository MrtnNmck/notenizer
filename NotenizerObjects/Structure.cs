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
    /// Persistable structure.
    /// </summary>
    public class Structure : IPersistable
    {
        #region Variables

        private DateTime _createdAt;
        private DateTime _updatedAt;
        private String _id;
        private NotenizerDependencies _dependencies;

        #endregion Variables

        #region Constructors

        public Structure()
        {
            _dependencies = new NotenizerDependencies();
        }

        public Structure(NotenizerDependencies dependencies, DateTime createdAt, DateTime updatedAt)
        {
            _dependencies = dependencies;
            _createdAt = createdAt;
            _updatedAt = updatedAt;
        }

        public Structure(NotenizerDependencies dependencies, String id, DateTime createdAt, DateTime updatedAt)
        {
            _dependencies = dependencies;
            _id = id;
            _createdAt = createdAt;
            _updatedAt = updatedAt;
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
        /// ID of document.
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
        /// Dependencies of structure.
        /// </summary>
        public NotenizerDependencies Dependencies
        {
            get
            {
                return _dependencies;
            }

            set
            {
                _dependencies = value;
            }
        }

        #endregion Properties

        #region Methods

        #endregion Methods
    }
}
