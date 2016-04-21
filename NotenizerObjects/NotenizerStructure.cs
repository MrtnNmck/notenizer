using nsEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsNotenizerObjects
{
    public class NotenizerStructure
    {
        private CompressedDependencies _compressedDependencies;
        private NotenizerDependencies _dependencies;
        private Structure _structure;

        public NotenizerStructure(NotenizerDependencies dependencies)
        {
            _dependencies = dependencies;
            _compressedDependencies = new CompressedDependencies(dependencies);
            _structure = new Structure(dependencies, DateTime.Now, DateTime.Now);
        }

        public NotenizerStructure(Structure structure)
        {
            this._structure = structure;
            this._dependencies = structure.Dependencies;
            this._compressedDependencies = new CompressedDependencies(this._dependencies);
        }

        public Structure Structure
        {
            get { return this._structure; }
            set { this._structure = value; }
        }

        public NotenizerDependencies Dependencies
        {
            get { return _dependencies; }
        }

        public int DistinctDependenciesCount
        {
            get { return _compressedDependencies.Keys.Count; }
        }

        public CompressedDependencies CompressedDependencies
        {
            get { return _compressedDependencies; }
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
                if (_compressedDependencies.ContainsKey(dependencyShortNameLoop))
                {
                    foreach (NotenizerDependency dependencyLoop in _compressedDependencies[dependencyShortNameLoop])
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
                if (_compressedDependencies.ContainsKey(dependencyShortNameLoop))
                {
                    foreach (NotenizerDependency dependencyLoop in _compressedDependencies[dependencyShortNameLoop])
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
                case ComparisonType.DependentToDependant:
                    return mainDependency.Dependent == secondaryDependency.Dependent;

                case ComparisonType.GovernorToGovernor:
                    return mainDependency.Governor == secondaryDependency.Governor;

                case ComparisonType.DependentToGovernor:
                    return mainDependency.Dependent == secondaryDependency.Governor;

                case ComparisonType.GovernorToDependant:
                    return mainDependency.Governor == secondaryDependency.Dependent;
            }

            throw new Exception("Error in CompareDependencies. Unidentified Comparison type.");
        }

        public NotenizerDependency FindDependency(NotenizerDependency ruleDependency)
        {
            if (_compressedDependencies.ContainsKey(ruleDependency.Relation.ShortName))
            {
                foreach (NotenizerDependency dependencyLoop in _compressedDependencies[ruleDependency.Relation.ShortName])
                {
                    //if (dependencyLoop.Governor.POS.Tag == ruleDependency.Governor.POS.Tag
                    //    && dependencyLoop.Governor.Index == ruleDependency.Governor.Index
                    //    && dependencyLoop.Dependent.POS.Tag == ruleDependency.Dependent.POS.Tag
                    //    && dependencyLoop.Dependent.Index == ruleDependency.Dependent.Index)
                    if (dependencyLoop.Governor.POS.Type == ruleDependency.Governor.POS.Type
                        && dependencyLoop.Dependent.POS.Type == ruleDependency.Dependent.POS.Type)
                        return dependencyLoop;
                }
            }

            return null;
        }

        public IEnumerable<NotenizerDependency> FindDependencies(NotenizerDependency ruleDependency)
        {
            if (_compressedDependencies.ContainsKey(ruleDependency.Relation.ShortName))
            {
                foreach (NotenizerDependency dependencyLoop in _compressedDependencies[ruleDependency.Relation.ShortName])
                {
                    if (dependencyLoop.Governor.POS.Type == ruleDependency.Governor.POS.Type
                        && dependencyLoop.Dependent.POS.Type == ruleDependency.Dependent.POS.Type)
                        yield return dependencyLoop;
                }
            }
        }

        public int DependencyWordsInSentenceCount()
        {
            int dependentMax = _dependencies.Max(x => x.Dependent.Index);
            int governorMax = _dependencies.Max(x => x.Governor.Index);

            return dependentMax > governorMax ? dependentMax : governorMax;
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
