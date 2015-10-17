using edu.stanford.nlp.pipeline;
using edu.stanford.nlp.trees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nsExtensions;

namespace nsNotenizerObjects
{
    public enum Comparison
    {
        GovernorToGovernor,
        GovernorToDependant,
        DependantToGovernor,
        DependantToDependant
    }

    public class NotenizerSentence
    {
		private List<NotenizerDependency> _dependencies;
		private Annotation _annotation;

		public NotenizerSentence(Annotation annotation)
		{
			_annotation = annotation;
			_dependencies = GetDepencencies(annotation);
		}

        public List<NotenizerDependency> Dependencies
        {
            get { return _dependencies; }
        }


        private List<NotenizerDependency> GetDepencencies(Annotation annotation)
		{
			Tree tree = annotation.get(typeof(TreeCoreAnnotations.TreeAnnotation)) as Tree;
			TreebankLanguagePack treeBankLangPack = new PennTreebankLanguagePack();
			GrammaticalStructureFactory gramStructFact = treeBankLangPack.grammaticalStructureFactory();
			GrammaticalStructure gramStruct = gramStructFact.newGrammaticalStructure(tree);
			java.util.Collection typedDependencies = gramStruct.typedDependenciesCollapsed();

            List<NotenizerDependency> dependencies = new List<NotenizerDependency>();

            foreach (TypedDependency typedDependencyLoop in (typedDependencies as java.util.ArrayList))
            {
                dependencies.Add(new NotenizerDependency(typedDependencyLoop));
            }

            return dependencies;
		}

        /// <summary>
        /// Gets first or null dependency by short name.
        /// </summary>
        /// <param name="dependencyShortName"></param>
        /// <param name="searchResult"></param>
        /// <returns></returns>
        public NotenizerDependency GetDependencyByShortName(NotenizerDependency mainDependency, Comparison comparisonType, params String[] dependencyShortNames)
        {
            foreach (NotenizerDependency dependencyLoop in _dependencies)
            {
                if (dependencyShortNames.Contains(dependencyLoop.Relation.ShortName))
                {
                    if (CompareDependencies(mainDependency, dependencyLoop, comparisonType))
                        return dependencyLoop;
                }
            }

            return null;
        }

        public List<NotenizerDependency> GetDependenciesByShortName(NotenizerDependency mainDependency, Comparison comparisonType, params String[] dependencyShortNames)
        {
            List<NotenizerDependency> dependencies = new List<NotenizerDependency>();

            foreach (NotenizerDependency dependencyLoop in _dependencies)
            {
                if (dependencyShortNames.Contains(dependencyLoop.Relation.ShortName))
                {
                    if (CompareDependencies(mainDependency, dependencyLoop, comparisonType))
                        dependencies.Add(dependencyLoop);
                }
            }

            return dependencies;
        }

        private bool CompareDependencies(NotenizerDependency mainDependency, NotenizerDependency secondaryDependency, Comparison comparisonType)
        {
            switch (comparisonType)
            {
                case Comparison.DependantToDependant:
                    return mainDependency.Dependant == secondaryDependency.Dependant;

                case Comparison.GovernorToGovernor:
                    return mainDependency.Governor == secondaryDependency.Governor;

                case Comparison.DependantToGovernor:
                    return mainDependency.Dependant == secondaryDependency.Governor;

                case Comparison.GovernorToDependant:
                    return mainDependency.Governor == secondaryDependency.Dependant;
            }

            throw new Exception("Error in CompareDependencies. Unidentified Comparison type.");
        }

        public override String ToString()
        {
            return _annotation.toString();
        }
    }
}
