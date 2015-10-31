using edu.stanford.nlp.pipeline;
using edu.stanford.nlp.trees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nsExtensions;
using nsEnums;

namespace nsNotenizerObjects
{
    public class NotenizerSentence
    {
		private List<NotenizerDependency> _dependencies;
        private Dictionary<String, List<NotenizerDependency>> _nameDependencyDic;
		private Annotation _annotation;

		public NotenizerSentence(Annotation annotation)
		{
			_annotation = annotation;
			_dependencies = GetDepencencies(annotation, ref _nameDependencyDic);
		}

        public List<NotenizerDependency> Dependencies
        {
            get { return _dependencies; }
        }

        public int DistinctDependenciesCount
        {
            get { return _nameDependencyDic.Keys.Count; }
        }

        public Dictionary<String, List<NotenizerDependency>> CompressedDependencies
        {
            get { return _nameDependencyDic; }
        }

        private List<NotenizerDependency> GetDepencencies(Annotation annotation, ref Dictionary<String, List<NotenizerDependency>> map)
		{
			Tree tree = annotation.get(typeof(TreeCoreAnnotations.TreeAnnotation)) as Tree;
			TreebankLanguagePack treeBankLangPack = new PennTreebankLanguagePack();
			GrammaticalStructureFactory gramStructFact = treeBankLangPack.grammaticalStructureFactory();
			GrammaticalStructure gramStruct = gramStructFact.newGrammaticalStructure(tree);
			java.util.Collection typedDependencies = gramStruct.typedDependenciesCollapsed();

            List<NotenizerDependency> dependencies = new List<NotenizerDependency>();
            map = new Dictionary<String, List<NotenizerDependency>>();

            foreach (TypedDependency typedDependencyLoop in (typedDependencies as java.util.ArrayList))
            {
                NotenizerDependency dep = new NotenizerDependency(typedDependencyLoop);

                dependencies.Add(dep);

                if (!map.ContainsKey(dep.Relation.ShortName))
                    map.Add(dep.Relation.ShortName, new List<NotenizerDependency>() { dep });
                else
                    map[dep.Relation.ShortName].Add(dep);
            }

            return dependencies;
		}

        /// <summary>
        /// Gets first or null dependency by short name.
        /// </summary>
        /// <param name="dependencyShortName"></param>
        /// <param name="searchResult"></param>
        /// <returns></returns>
        public NotenizerDependency GetDependencyByShortName(NotenizerDependency mainDependency, ComparisonType comparisonType, params String[] dependencyShortNames)
        {
            foreach (String dependencyShortNameLoop in dependencyShortNames)
            {
                if (_nameDependencyDic.ContainsKey(dependencyShortNameLoop))
                {
                    foreach (NotenizerDependency dependencyLoop in _nameDependencyDic[dependencyShortNameLoop])
                    {
                        if (CompareDependencies(mainDependency, dependencyLoop, comparisonType))
                        {
                            dependencyLoop.ComparisonType = comparisonType;
                            return dependencyLoop;
                        }
                    }
                }
            }

            return null;
        }

        public List<NotenizerDependency> GetDependenciesByShortName(NotenizerDependency mainDependency, ComparisonType comparisonType, params String[] dependencyShortNames)
        {
            List<NotenizerDependency> dependencies = new List<NotenizerDependency>();

            foreach (String dependencyShortNameLoop in dependencyShortNames)
            {
                if (_nameDependencyDic.ContainsKey(dependencyShortNameLoop))
                {
                    foreach (NotenizerDependency dependencyLoop in _nameDependencyDic[dependencyShortNameLoop])
                    {
                        if (CompareDependencies(mainDependency, dependencyLoop, comparisonType))
                        {
                            dependencyLoop.ComparisonType = comparisonType;
                            dependencies.Add(dependencyLoop);
                        }
                    }
                }
            }

            return dependencies;
        }

        private bool CompareDependencies(NotenizerDependency mainDependency, NotenizerDependency secondaryDependency, ComparisonType comparisonType)
        {
            switch (comparisonType)
            {
                case ComparisonType.DependantToDependant:
                    return mainDependency.Dependent == secondaryDependency.Dependent;

                case ComparisonType.GovernorToGovernor:
                    return mainDependency.Governor == secondaryDependency.Governor;

                case ComparisonType.DependantToGovernor:
                    return mainDependency.Dependent == secondaryDependency.Governor;

                case ComparisonType.GovernorToDependant:
                    return mainDependency.Governor == secondaryDependency.Dependent;
            }

            throw new Exception("Error in CompareDependencies. Unidentified Comparison type.");
        }

        public NotenizerDependency FindDependency(NotenizerDependency ruleDependency)
        {
            if (_nameDependencyDic.ContainsKey(ruleDependency.Relation.ShortName))
            {
                foreach (NotenizerDependency dependencyLoop in _nameDependencyDic[ruleDependency.Relation.ShortName])
                {
                    if (dependencyLoop.Governor.POS == ruleDependency.Governor.POS
                        && dependencyLoop.Governor.Index == ruleDependency.Governor.Index
                        && dependencyLoop.Dependent.POS == ruleDependency.Dependent.POS
                        && dependencyLoop.Dependent.Index == ruleDependency.Dependent.Index)
                        return dependencyLoop;
                }
            }

            return null;
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
