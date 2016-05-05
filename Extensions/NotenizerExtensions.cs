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
    /// <summary>
    /// Extension methods for the StanfordCoreNLP and the Notenizer objects.
    /// </summary>
    public static class NotenizerExtensions
    {
        #region Methods

        /// <summary>
        /// Created Comaprison type from two token types.
        /// </summary>
        /// <param name="leftSide"></param>
        /// <param name="rigthSide"></param>
        /// <returns></returns>
        public static ComparisonType CreateComperisonType(this TokenType leftSide, TokenType rigthSide)
        {
            return (ComparisonType)Enum.Parse(typeof(ComparisonType), leftSide.ToString() + "To" + rigthSide.ToString());
        }

        /// <summary>
        /// Compares grammatical relations.
        /// </summary>
        /// <param name="gr1"></param>
        /// <param name="gr2"></param>
        /// <returns></returns>
        public static bool IsGrammaticalRelation(this GrammaticalRelation gr1, GrammaticalRelation gr2)
        {
            return gr1.getLongName() == gr2.getLongName();
        }

        /// <summary>
        /// Gets unique identifier of IndexedWord
        /// </summary>
        /// <param name="idxWord"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Filter TypedDepenencies byt Part-of-Speech tags.
        /// </summary>
        /// <param name="dependencies">List of TypeDependency</param>
        /// <param name="poses">Part-of-speech tags</param>
        /// <returns></returns>
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

        /// <summary>
        /// Adjusts speciic of GrammaticalRelation.
        /// </summary>
        /// <param name="relation"></param>
        /// <returns></returns>
        public static String GetAdjustedSpecific(this GrammaticalRelation relation)
        {
            return relation.getSpecific().AdjustSpecific();
        }

        /// <summary>
        /// Adjusts speciic of GrammaticalRelation.
        /// </summary>
        /// <param name="specific"></param>
        /// <returns></returns>
        public static String AdjustSpecific(this String specific)
        {
            if (specific == "agent")
                return "by";

            return specific;
        }

        /// <summary>
        /// Converts string to Bson ObjectId.
        /// </summary>
        /// <param name="objectIdString"></param>
        /// <returns></returns>
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
