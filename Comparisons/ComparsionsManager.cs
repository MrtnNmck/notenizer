using nsConstants;
using nsNotenizerObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsComparsions
{
    public class ComparsionsManager
    {
        private Comparsions<bool> _boolWordsComparsions;
        private Comparsions<double> _doubleWordsComparsions;
        //private Comparsions<bool> _boolDependenciesComparsions;
        //private Comparsions<double> _doubleDependenciesComparsions;
        private double _oneComparisonRating;
        private double _matchRating;

        public ComparsionsManager()
        {
            Init();
        }

        private void Init()
        {
            InitWordsComparsions();
            //InitDependenicesComparsions();
        }

        private void InitWordsComparsions()
        {
            _boolWordsComparsions = new Comparsions<bool>();
            _boolWordsComparsions.Add(new Comparsion<bool>(nsComparsions.Functions.ComparsionFunctions.SamePartOfSpeechTag));
            _boolWordsComparsions.Add(new Comparsion<bool>(nsComparsions.Functions.ComparsionFunctions.SameNER));
            _boolWordsComparsions.Add(new Comparsion<bool>(nsComparsions.Functions.ComparsionFunctions.SameIndex));
            _boolWordsComparsions.Add(new Comparsion<bool>(nsComparsions.Functions.ComparsionFunctions.SameLemma));

            _doubleWordsComparsions = new Comparsions<double>();
            _doubleWordsComparsions.Add(new Comparsion<double>(nsComparsions.Functions.ComparsionFunctions.CalculateClosenessWords));
        }

        //private void InitDependenicesComparsions()
        //{
        //    _boolDependenciesComparsions = new Comparsions<bool>();
        //    _boolDependenciesComparsions.Add(new Comparsion<bool>(nsComparsions.Functions.ComparsionFunctions.SamePosition));

        //    _doubleDependenciesComparsions = new Comparsions<double>();
        //    _doubleDependenciesComparsions.Add(new Comparsion<double>(nsComparsions.Functions.ComparsionFunctions.CalculateClosenessDependencies));
        //}

        public double Compare(NotenizerDependency source, NotenizerDependency dest, int dependeciesAndWordsCount)
        {
            _matchRating = 0;
            _oneComparisonRating = NotenizerConstants.MaximumMatchPercentageValue / Count;

            foreach (Comparsion<bool> boolWordComparsionLoop in _boolWordsComparsions)
            {
                if (boolWordComparsionLoop.Run(source.Governor, dest.Governor))
                    _matchRating += _oneComparisonRating;

                if (boolWordComparsionLoop.Run(source.Dependent, dest.Dependent))
                    _matchRating += _oneComparisonRating;
            }

            foreach (Comparsion<double> doubleWordsComparsionLoop in _doubleWordsComparsions)
            {
                _matchRating += doubleWordsComparsionLoop.Run(source.Governor, dest.Governor, _oneComparisonRating, dependeciesAndWordsCount);
                _matchRating += doubleWordsComparsionLoop.Run(source.Dependent, dest.Dependent, _oneComparisonRating, dependeciesAndWordsCount);
            }

            //foreach (Comparsion<bool> boolDependenciesComparsionLoop in _boolDependenciesComparsions)
            //{
            //    if (boolDependenciesComparsionLoop.Run(source, dest))
            //        _matchRating += _oneComparisonRating;
            //}

            //foreach (Comparsion<double> doubleDependenciesComparsionLoop in _doubleDependenciesComparsions)
            //{
            //    _matchRating += doubleDependenciesComparsionLoop.Run(source, dest, _oneComparisonRating, dependeciesAndWordsCount);
            //}

            return _matchRating;
        }

        private int Count
        {
            get
            {
                return 10;
            }
        }
    }
}
