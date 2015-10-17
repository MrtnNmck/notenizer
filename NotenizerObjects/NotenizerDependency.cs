using edu.stanford.nlp.trees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsNotenizerObjects
{
	public class NotenizerDependency
	{
		private NotenizerWord _dependant;
		private NotenizerWord _governor;
		private NotenizerRelation _relation;

        public NotenizerDependency(TypedDependency typedDependency)
        {
            _dependant = new NotenizerWord(typedDependency.dep());
            _governor = new NotenizerWord(typedDependency.gov());
            _relation = new NotenizerRelation(typedDependency.reln());
        }

        public NotenizerWord Dependant
        {
            get { return _dependant; }
        }

        public NotenizerWord Governor
        {
            get { return _governor; }
        }

        public NotenizerRelation Relation
        {
            get { return _relation; }
        }
	}
}
