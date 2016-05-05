using nsConstants;
using nsNotenizerObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsComparsions
{
    /// <summary>
    /// Manages comparsions.
    /// </summary>
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

        /// <summary>
        /// Number of comarsions.
        /// </summary>
        private int Count
        {
            get
            {
                return 10;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Initializes ComparisonManager.
        /// </summary>
        private void Init()
        {
            InitWordsComparsions();
        }

        /// <summary>
        /// Initializes words comparsions.
        /// </summary>
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

        /// <summary>
        /// Compares dependencies.
        /// </summary>
        /// <param name="source">Source dependency</param>
        /// <param name="dest">Destination depenedency</param>
        /// <param name="dependeciesAndWordsCount">Number of dependencies and words</param>
        /// <returns></returns>
        public double Compare(NotenizerDependency source, NotenizerDependency dest, int dependeciesAndWordsCount)
        {
            try
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
            catch (Exception ex)
            {
                throw new Exception("Error comnparing two dependencies." + Environment.NewLine + Environment.NewLine + ex.Message);
            }
        }

        #endregion Methods
    }
}
