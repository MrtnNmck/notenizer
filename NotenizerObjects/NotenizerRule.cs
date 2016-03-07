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

        public NotenizerRule(List<NotenizerDependency> dependencies, List<int> sentencesEnds, CreatedBy createdBy)
        {
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
    }
}
