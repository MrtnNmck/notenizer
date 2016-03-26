using nsNotenizerObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsComparsions.Functions
{
    public static partial class ComparsionFunctions
    {
        public static bool SamePartOfSpeechTag(NotenizerWord firstW, NotenizerWord secondW)
        {
            return firstW.POS.Tag == secondW.POS.Tag;
        }

        public static bool SameNER(NotenizerWord firstW, NotenizerWord secondW)
        {
            return firstW.NamedEntity.Value == secondW.NamedEntity.Value;
        }

        public static bool SameIndex(NotenizerWord firstW, NotenizerWord secondW)
        {
            return firstW.Index == secondW.Index;
        }

        public static bool SameLemma(NotenizerWord firstW, NotenizerWord secondW)
        {
            return firstW.Lemma == secondW.Lemma;
        }

        public static bool SamePosition(NotenizerDependency firstD, NotenizerDependency secondD)
        {
            return firstD.Position == secondD.Position;
        }

        public static double CalculateClosenessWords(NotenizerWord firstW, NotenizerWord secondW, double comparisonValue, int count)
        {
            return CalculateCloseness(firstW.Index, secondW.Index, comparisonValue, count);
        }

        public static double CalculateClosenessDependencies(NotenizerDependency firstDep, NotenizerDependency secondDep, double comparisonValue, int count)
        {
            return CalculateCloseness(firstDep.Position, secondDep.Position, comparisonValue, count);
        }

        private static double CalculateCloseness(int firstIndex, int secondIndex, double comparisonValue, int count)
        {
            return comparisonValue - (Math.Abs(firstIndex - secondIndex) * (comparisonValue / count));
        }
    }
}
