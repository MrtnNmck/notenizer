using nsNotenizerObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsComparsions
{
    /// <summary>
    /// Generic class for comparsion.
    /// </summary>
    /// <typeparam name="T">Return type of comparsion</typeparam>
    public class Comparsion<T>
    {
        #region Variables

        private Func<NotenizerWord, NotenizerWord, T> _wordsComparisonFunction;
        private Func<NotenizerDependency, NotenizerDependency, T> _dependenciesComparsionFunction;
        private Func<NotenizerWord, NotenizerWord, double, int, T> _wordsClosenessFunction;
        private Func<NotenizerDependency, NotenizerDependency, double, int, T> _dependenciesClosenessFunction;

        #endregion Variables

        #region Constructors
        public Comparsion(Func<NotenizerWord, NotenizerWord, T> comparsionFunction)
        {
            _wordsComparisonFunction = comparsionFunction;
        }

        public Comparsion(Func<NotenizerDependency, NotenizerDependency, T> comparsionFunction)
        {
            _dependenciesComparsionFunction = comparsionFunction;
        }

        public Comparsion(Func<NotenizerWord, NotenizerWord, double, int, T> comparsionFunction)
        {
            _wordsClosenessFunction = comparsionFunction;
        }

        public Comparsion(Func<NotenizerDependency, NotenizerDependency, double, int, T> comparsionFunction)
        {
            _dependenciesClosenessFunction = comparsionFunction;
        }

        #endregion Constuctors

        #region Properties

        #endregion Properties

        #region Methods

        /// <summary>
        /// Runs words comparsion.
        /// </summary>
        /// <param name="source">Source word</param>
        /// <param name="destination">Destination word</param>
        /// <returns></returns>
        public T Run(NotenizerWord source, NotenizerWord destination)
        {
            return _wordsComparisonFunction(source, destination);
        }

        /// <summary>
        /// Runs words comparsion.
        /// </summary>
        /// <param name="source">Source word</param>
        /// <param name="dest">Destination wors</param>
        /// <param name="rating">Rating of comparsion</param>
        /// <param name="count">Count</param>
        /// <returns></returns>
        public T Run(NotenizerWord source, NotenizerWord dest, double rating, int count)
        {
            return _wordsClosenessFunction(source, dest, rating, count);
        }

        /// <summary>
        /// Runs dependencies comparsion.
        /// </summary>
        /// <param name="source">Source dependency</param>
        /// <param name="dest">Destination dependency</param>
        /// <returns></returns>
        public T Run(NotenizerDependency source, NotenizerDependency dest)
        {
            return _dependenciesComparsionFunction(source, dest);
        }

        /// <summary>
        /// Runs dependencies comparsion.
        /// </summary>
        /// <param name="source">Source dependency</param>
        /// <param name="dest">Destination dependency</param>
        /// <param name="rating">rating of comparsion</param>
        /// <param name="count">Count</param>
        /// <returns></returns>
        public T Run(NotenizerDependency source, NotenizerDependency dest, double rating, int count)
        {
            return _dependenciesClosenessFunction(source, dest, rating, count);
        }

        #endregion Methods
    }
}
