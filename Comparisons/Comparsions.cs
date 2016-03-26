using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsComparsions
{
    public class Comparsions<T> : List<Comparsion<T>>
    {
        //private List<Comparsion<bool>> _boolWordsComparsions;
        //private List<Comparsion<double>> _doubleWordsComparsions;
        //private List<Comparsion<bool>> _boolDependenciesComparsions;
        //private List<Comparsion<double>> _doublDependenciesComparsions;

        public Comparsions()
        {
            //Init();
        }

        //private void Init()
        //{
        //    InitWordsComparsions();
        //    InitDependenicesComparsions();            
        //}

        //private void InitWordsComparsions()
        //{
        //    _boolWordsComparsions = new List<Comparsion<bool>>();
        //    _boolWordsComparsions.Add(new Comparsion<bool>(nsComparsions.Functions.ComparsionFunctions.SamePartOfSpeechTag));
        //    _boolWordsComparsions.Add(new Comparsion<bool>(nsComparsions.Functions.ComparsionFunctions.SameWordValue));
        //    _boolWordsComparsions.Add(new Comparsion<bool>(nsComparsions.Functions.ComparsionFunctions.SameIndex));
        //    _boolWordsComparsions.Add(new Comparsion<bool>(nsComparsions.Functions.ComparsionFunctions.SameLemma));
        //    _boolWordsComparsions.Add(new Comparsion<bool>(nsComparsions.Functions.ComparsionFunctions.SameWordValue));

        //    _doubleWordsComparsions = new List<Comparsion<double>>();
        //    _doubleWordsComparsions.Add(new Comparsion<double>(nsComparsions.Functions.ComparsionFunctions.CalculateClosenessWords));
        //}

        //private void InitDependenicesComparsions()
        //{
        //    _boolDependenciesComparsions = new List<Comparsion<bool>>();
        //    _boolDependenciesComparsions.Add(new Comparsion<bool>(nsComparsions.Functions.ComparsionFunctions.SamePosition));

        //    _doubleWordsComparsions = new List<Comparsion<double>>();
        //    _doubleWordsComparsions.Add(new Comparsion<double>(nsComparsions.Functions.ComparsionFunctions.CalculateClosenessDependencies));
        //}
    }
}
