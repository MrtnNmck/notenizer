using nsEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsNotenizerObjects
{
    public class NotenizerRule
    {
        protected List<NotenizerDependency> _dependencies;
        protected Double _match;
        protected CreatedBy _createdBy;
        protected String _id;
        protected String _noteId;
        protected DateTime _createdAt;
        protected DateTime _updatedAt;

        public NotenizerRule(String id, List<NotenizerDependency> dependencies, CreatedBy createdBy)
        {
            _id = id;
            _dependencies = dependencies;
            _createdBy = createdBy;
            _createdAt = DateTime.Now;
            _updatedAt = DateTime.Now;
        }

        public List<NotenizerDependency> RuleDependencies
        {
            get { return _dependencies; }
        }

        public Double Match
        {
            get { return _match; }
            set { _match = value; }
        }

        public CreatedBy CreatedBy
        {
            get { return _createdBy; }
        }

        public DateTime CreatedAt
        {
            set { _createdAt = value; }
            get { return _createdAt; }
        }

        public DateTime UpdatedAt
        {
            set { _updatedAt = value; }
            get { return _updatedAt; }
        }

        /// <summary>
        /// ID of corresponding entry in DB, from which this rule is.
        /// </summary>
        public String ID
        {
            get { return _id; }
        }

        public String NoteID
        {
            get { return _noteId; }
            set { _noteId = value; }
        }
    }
}
