using MongoDB.Bson;
using nsConstants;
using nsEnums;
using nsExtensions;
using nsNotenizerObjects;
using System.Collections.Generic;

namespace nsNotenizer
{
    public static class DocumentParser
    {
        public static NotenizerRule ParseNoteDependencies(BsonDocument dbEntry)
        {
            List<NotenizerDependency> dependencies = new List<NotenizerDependency>();

            // foreach note dependency
            foreach (BsonDocument documentLoop in dbEntry[DBConstants.NoteDependenciesFieldName].AsBsonArray)
            {
                NotenizerRelation relation = new NotenizerRelation(documentLoop[DBConstants.DependencyNameFieldName].AsString);

                // foreach dependency in array of dependencies with same relation name
                foreach (BsonDocument dependencyDocLoop in documentLoop[DBConstants.DependenciesFieldName].AsBsonArray)
                {
                    BsonDocument governorDoc = dependencyDocLoop[DBConstants.GovernorFieldName].AsBsonDocument;
                    BsonDocument dependantDoc = dependencyDocLoop[DBConstants.DependentFieldName].AsBsonDocument;
                    int position = dependencyDocLoop[DBConstants.PositionFieldName].AsInt32;
                    ComparisonType comparisonType = dependencyDocLoop[DBConstants.ComparisonTypeFieldName].AsInt32.ToEnum<ComparisonType>();

                    NotenizerWord governor = new NotenizerWord(governorDoc[DBConstants.POSFieldName].AsString, governorDoc[DBConstants.IndexFieldName].AsInt32);
                    NotenizerWord dependent = new NotenizerWord(dependantDoc[DBConstants.POSFieldName].AsString, dependantDoc[DBConstants.IndexFieldName].AsInt32);

                    NotenizerDependency dependency = new NotenizerDependency(governor, dependent, relation, position, comparisonType);

                    dependencies.Add(dependency);
                }
            }

            List<int> sentencesEnds = new List<int>();
            foreach (BsonInt32 endLoop in dbEntry[DBConstants.SentencesEndsFieldName].AsBsonArray)
                sentencesEnds.Add((int)endLoop);

            return new NotenizerRule(dependencies, sentencesEnds);
        }
    }
}
