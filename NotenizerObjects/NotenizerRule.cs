using MongoDB.Bson;
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
        private List<NotenizerDependency> _dependencies;
        private List<int> _sentencesEnds;
        private Double _match;
        private CreatedBy _createdBy;
        private BsonObjectId _objectId;

        public NotenizerRule(BsonObjectId id, List<NotenizerDependency> dependencies, List<int> sentencesEnds, CreatedBy createdBy)
        {
            _objectId = id;
            _dependencies = dependencies;
            _sentencesEnds = sentencesEnds;
            _createdBy = createdBy;
        }

        public List<NotenizerDependency> RuleDependencies
        {
            get { return _dependencies; }
        }

        public List<int> SentencesEnds
        {
            get { return _sentencesEnds; }
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

        /// <summary>
        /// ID of corresponding entry in DB, from which this rule is.
        /// </summary>
        public BsonObjectId ID
        {
            get { return _objectId; }
        }
    }
}
