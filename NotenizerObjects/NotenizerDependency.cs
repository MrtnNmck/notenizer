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
		private NotenizerWord _dependent;
		private NotenizerWord _governor;
		private NotenizerRelation _relation;
        private TypedDependency _originalDependency;

        public NotenizerDependency(TypedDependency typedDependency)
        {
            _dependent = new NotenizerWord(typedDependency.dep());
            _governor = new NotenizerWord(typedDependency.gov());
            _relation = new NotenizerRelation(typedDependency.reln());
            _originalDependency = typedDependency;
        }

        public NotenizerWord Dependent
        {
            get { return _dependent; }
        }

        public NotenizerWord Governor
        {
            get { return _governor; }
        }

        public NotenizerRelation Relation
        {
            get { return _relation; }
        }

        public TypedDependency OriginalDependency
        {
            get { return _originalDependency; }
        }

        public String Key
        {
            get { return _relation.ShortName + Governor.Word + Governor.Index + Dependent.Word + Dependent.Index; }
        }
	}
}
