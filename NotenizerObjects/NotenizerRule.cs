using nsEnums;
using nsInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsNotenizerObjects
{
    /// <summary>
    /// Persistable rule
    /// </summary>
    public class NotenizerRule : IPersistable
    {
        #region Variables

        protected NotenizerDependencies _dependencies;
        protected Match _match;
        protected String _id;
        protected DateTime _createdAt;
        protected DateTime _updatedAt;
        protected NotenizerStructure _structure;
        protected String _structureID;
        protected Sentence _sentence;

        #endregion Variables

        #region Constructors

        public NotenizerRule(String id, NotenizerDependencies dependencies)
        {
            _id = id;
            _structure = new NotenizerStructure(dependencies);
            _dependencies = dependencies;
            _createdAt = DateTime.Now;
            _updatedAt = DateTime.Now;
        }

        public NotenizerRule()
        {
            _id = null;
            _dependencies = null;
            _createdAt = DateTime.Now;
            _updatedAt = DateTime.Now;
        }

        public NotenizerRule(String id, String structureId, DateTime createdAt, DateTime updatedAt)
        {
            this._id = id;
            this._structureID = structureId;
            this._createdAt = createdAt;
            this._updatedAt = updatedAt;
        }

        #endregion Constuctors

        #region Properties

        /// <summary>
        /// Dependencies of rule.
        /// </summary>
        public NotenizerDependencies RuleDependencies
        {
            get { return _dependencies; }
            set { _dependencies = value; }
        }

        /// <summary>
        /// Match of structures.
        /// </summary>
        public Match Match
        {
            get { return _match; }
            set { _match = value; }
        }

        /// <summary>
        /// Created at timestamp.
        /// </summary>
        public DateTime CreatedAt
        {
            set { _createdAt = value; }
            get { return _createdAt; }
        }

        /// <summary>
        /// Updated at timestamp.
        /// </summary>
        public DateTime UpdatedAt
        {
            set { _updatedAt = value; }
            get { return _updatedAt; }
        }

        /// <summary>
        /// ID of corresponding entry document in DB.
        /// </summary>
        public String ID
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Structure of rule.
        /// </summary>
        public NotenizerStructure Structure
        {
            get { return this._structure; }
            set { this._structure = value; }
        }

        /// <summary>
        /// ID of structure.
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
        /// Sentence of rule.
        /// </summary>
        public Sentence Sentence
        {
            get
            {
                return _sentence;
            }

            set
            {
                _sentence = value;
            }
        }

        #endregion Properties

        #region Methods

        #endregion Methods
    }
}
