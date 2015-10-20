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
                    return mainDependency.Dependent == secondaryDependency.Dependent;

                case Comparison.GovernorToGovernor:
                    return mainDependency.Governor == secondaryDependency.Governor;

                case Comparison.DependantToGovernor:
                    return mainDependency.Dependent == secondaryDependency.Governor;

                case Comparison.GovernorToDependant:
                    return mainDependency.Governor == secondaryDependency.Dependent;
            }

            throw new Exception("Error in CompareDependencies. Unidentified Comparison type.");
        }

        public List<String> CopyStructure()
        {
            List<String> structure = new List<String>();

            foreach (NotenizerDependency dependencyLoop in _dependencies)
                structure.Add(dependencyLoop.Key);

            return structure;
        }

        public int DependencyWordsInSentenceCount()
        {
            int dependentMax = _dependencies.Max(x => x.Dependent.Index);
            int governorMax = _dependencies.Max(x => x.Governor.Index);

            return dependentMax > governorMax ? dependentMax : governorMax;
        }

        public override String ToString()
        {
            return _annotation.toString();
        }

        public int DependencyIndex(NotenizerDependency dependency)
        {
            for (int i = 0; i < _dependencies.Count; i++)
            {
                if (dependency.Dependent.Word == _dependencies[i].Dependent.Word
                    && dependency.Governor.Word == _dependencies[i].Governor.Word
                    && dependency.Relation.ShortName == _dependencies[i].Relation.ShortName
                    && dependency.Dependent.Index == _dependencies[i].Dependent.Index
                    && dependency.Governor.Index == _dependencies[i].Governor.Index)
                    return i;
            }

            throw new Exception("Sentence doesn't contain dependency: " + dependency.ToString());
        }
    }
}
