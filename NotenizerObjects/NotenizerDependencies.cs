using nsConstants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsNotenizerObjects
{
    /// <summary>
    /// List of the Notenizer Dependencies.
    /// </summary>
    public class NotenizerDependencies : List<NotenizerDependency>
    {
        #region Variables

        #endregion Variables

        #region Constructors

        public NotenizerDependencies()
        {
        }

        #endregion Constuctors

        #region Properties

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets complement of the Notenizer Dependencies.
        /// </summary>
        /// <param name="sourceDependencies"></param>
        /// <param name="sourceCompressedDependencies"></param>
        /// <returns></returns>
        public NotenizerDependencies Complement(NotenizerDependencies sourceDependencies, CompressedDependencies sourceCompressedDependencies)
        {
            NotenizerDependencies unusedDependencies = new NotenizerDependencies();

            foreach (NotenizerDependency originalDependencyLoop in sourceDependencies)
            {
                if (!originalDependencyLoop.Relation.IsRelation(GrammaticalConstants.Root) && !this.Exists(x => x.Key == originalDependencyLoop.Key))
                    unusedDependencies.Add(originalDependencyLoop);
            }

            // we need to check, if both tokens of NSUBJ / NSUBJPASS were in note dependencies
            // if not, we need to add the other one to the unused dependencies.
            IEnumerable<NotenizerDependency> nsubjDependencies = new List<NotenizerDependency>();

            if (sourceCompressedDependencies.ContainsKey(GrammaticalConstants.NominalSubject))
                nsubjDependencies = sourceCompressedDependencies[GrammaticalConstants.NominalSubject];

            if (sourceCompressedDependencies.ContainsKey(GrammaticalConstants.NominalSubjectPassive))
                nsubjDependencies = nsubjDependencies.Concat(sourceCompressedDependencies[GrammaticalConstants.NominalSubjectPassive]);

            foreach (NotenizerDependency nsubjDependencyLoop in nsubjDependencies)
            {
                if (!this.Exists(x => x.Key == nsubjDependencyLoop.Key && x.TokenType == nsubjDependencyLoop.TokenType)
                    && !unusedDependencies.Exists(x => x.Key == nsubjDependencyLoop.Key && x.TokenType == nsubjDependencyLoop.TokenType))
                    unusedDependencies.Add(nsubjDependencyLoop);
            }

            return unusedDependencies;
        }

        #endregion Methods
    }
}
