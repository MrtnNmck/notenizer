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
        #region Variables

        private Comparsions<bool> _boolWordsComparsions;
        private Comparsions<double> _doubleWordsComparsions;
        private double _oneComparisonRating;
        private double _matchRating;

        #endregion Variables

        #region Constructors
        public ComparsionsManager()
        {
            Init();
        }

        #endregion Constuctors

        #region Properties
        private int Count
        {
            get
            {
                return 10;
            }
        }

        #endregion Properties

        #region Event Handlers

        #endregion Event Handlers

        #region Methods

        private void Init()
        {
            InitWordsComparsions();
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

        public double Compare(NotenizerDependency source, NotenizerDependency dest, int dependeciesAndWordsCount)
        {
            _matchRating = 0;
            _oneComparisonRating = NotenizerConstants.MaxMatchValue / Count;

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

            return _matchRating;
        }

        #endregion Methods
    }
}
