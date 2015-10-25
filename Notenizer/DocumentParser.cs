using MongoDB.Bson;
using nsConstants;
using nsEnums;
using nsExtensions;
using nsNotenizerObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsNotenizer
{
    public static class DocumentParser
    {
        public static List<NotenizerDependency> ParseNoteDependencies(BsonDocument dbEntry)
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

            return dependencies;
        }
    }
}
