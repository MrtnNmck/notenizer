using nsNotenizerObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsComparsions
{
    public class Comparsion<T>
    {
        private Func<NotenizerWord, NotenizerWord, T> _wordsComparisonFunction;
        private Func<NotenizerDependency, NotenizerDependency, T> _dependenciesComparsionFunction;
        private Func<NotenizerWord, NotenizerWord, double, int, T> _wordsClosenessFunction;
        private Func<NotenizerDependency, NotenizerDependency, double, int, T> _dependenciesClosenessFunction;

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

        public T Run(NotenizerWord source, NotenizerWord destination)
        {
            return _wordsComparisonFunction(source, destination);
        }

        public T Run(NotenizerWord source, NotenizerWord dest, double rating, int count)
        {
            return _wordsClosenessFunction(source, dest, rating, count);
        }

        public T Run(NotenizerDependency source, NotenizerDependency dest)
        {
            return _dependenciesComparsionFunction(source, dest);
        }

        public T Run(NotenizerDependency source, NotenizerDependency dest, double rating, int count)
        {
            return _dependenciesClosenessFunction(source, dest, rating, count);
        }
    }
}
