using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsNotenizerObjects
{
    /// <summary>
    /// Compressed notenizer dependencies by short name.
    /// </summary>
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

        /// <summary>
        /// Adds dependency.
        /// </summary>
        /// <param name="dependency"></param>
        public void Add(NotenizerDependency dependency)
        {
            if (!this.ContainsKey(dependency.Relation.ShortName))
                this.Add(dependency.Relation.ShortName, new NotenizerDependencies() { dependency });
            else
                this[dependency.Relation.ShortName].Add(dependency);
        }

        /// <summary>
        /// Converts NotenizerDependencies to CompressedDepenendcies.
        /// </summary>
        /// <param name="dependencies"></param>
        private void Convert(NotenizerDependencies dependencies)
        {
            foreach (NotenizerDependency dependencyLoop in dependencies)
                this.Add(dependencyLoop);
        }

        #endregion Methods
    }
}
