using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsNotenizerObjects
{
    public class CompressedDependencies : Dictionary<String, NotenizerDependencies>
    {
        #region Variables

        #endregion Variables

        #region Constructors

        public CompressedDependencies()
        {
        }

        public CompressedDependencies(NotenizerDependencies dependencies)
        {
            Convert(dependencies);
        }

        #endregion Constuctors

        #region Properties

        #endregion Properties

        #region Methods

        public void Add(NotenizerDependency dependency)
        {
            if (!this.ContainsKey(dependency.Relation.ShortName))
                this.Add(dependency.Relation.ShortName, new NotenizerDependencies() { dependency });
            else
                this[dependency.Relation.ShortName].Add(dependency);
        }

        private void Convert(NotenizerDependencies dependencies)
        {
            foreach (NotenizerDependency dependencyLoop in dependencies)
                this.Add(dependencyLoop);
        }

        #endregion Methods
    }
}
