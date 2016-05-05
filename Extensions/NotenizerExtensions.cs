using edu.stanford.nlp.ling;
using edu.stanford.nlp.trees;
using MongoDB.Bson;
using nsEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsExtensions
{
    public static class NotenizerExtensions
    {
        #region Methods

        public static ComparisonType CreateComperisonType(this TokenType leftSide, TokenType rigthSide)
        {
            return (ComparisonType)Enum.Parse(typeof(ComparisonType), leftSide.ToString() + "To" + rigthSide.ToString());
        }

        public static bool IsGrammaticalRelation(this GrammaticalRelation gr1, GrammaticalRelation gr2)
        {
            return gr1.getLongName() == gr2.getLongName();
        }

        public static String GetUniqueIdentifier(this IndexedWord idxWord)
        {
            return idxWord.word()
                + "/"
                + idxWord.get(typeof(CoreAnnotations.PartOfSpeechAnnotation)).ToString()
                + "/"
                + idxWord.beginPosition()
                + "/"
                + idxWord.endPosition();
        }

        public static List<TypedDependency> FilterByPOS(this List<TypedDependency> dependencies, String[] poses)
        {
            List<TypedDependency> filteredDependencies = new List<TypedDependency>();

            foreach (TypedDependency dependencyLoop in dependencies)
            {
                if (poses.Contains(dependencyLoop.dep().get(typeof(CoreAnnotations.PartOfSpeechAnnotation)).ToString()))
                    filteredDependencies.Add(dependencyLoop);
            }

            return filteredDependencies;
        }

        public static String GetAdjustedSpecific(this GrammaticalRelation relation)
        {
            return relation.getSpecific().AdjustSpecific();
        }

        public static String AdjustSpecific(this String specific)
        {
            if (specific == "agent")
                return "by";

            return specific;
        }

        public static BsonValue ToObjectId(this String objectIdString)
        {
            try
            {
                if (objectIdString == String.Empty)
                    return BsonNull.Value;

                return ObjectId.Parse(objectIdString);
            }
            catch (Exception ex)
            {
                throw new Exception("Error parsing ObjectId from string " + objectIdString + Environment.NewLine + Environment.NewLine + ex.Message);
            }
        }

        #endregion Methods
    }
}
