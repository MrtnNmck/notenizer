using MongoDB.Bson;
using nsConstants;
using nsEnums;
using nsExtensions;
using nsNotenizerObjects;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public static NotenizerRule GetHeighestMatch(NotenizerSentence sentence, List<BsonDocument> dbEntries)
        {
            NotenizerRule rule = null;
            foreach (BsonDocument bsonDocLoop in dbEntries)
            {
                NotenizerRule r = ParseNoteDependencies(bsonDocLoop);

                r.Match = CalculateMatch(sentence, r.RuleDependencies, bsonDocLoop);

                if (rule == null || rule.Match < r.Match)
                    rule = r;
            }

            return rule;
        }

        private static Double CalculateMatch(NotenizerSentence sentence, List<NotenizerDependency> parsedDependencies, BsonDocument dbEntry)
        {
            Double compareCount = 8.0;
            Double oneCompareType = 100.0 / compareCount;
            Double oneCompareTypeIter;
            Double counter = 0.0;

            int c = 0;

            oneCompareTypeIter = oneCompareType / Double.Parse(dbEntry[DBConstants.OriginalSentenceDependenciesFieldName].AsBsonArray.Count.ToString());
            foreach (BsonDocument origDepDocLoop in dbEntry[DBConstants.OriginalSentenceDependenciesFieldName].AsBsonArray)
            {
                String depName = origDepDocLoop[DBConstants.DependencyNameFieldName].AsString;

                if (sentence.CompressedDependencies[depName].Count == origDepDocLoop[DBConstants.DependenciesFieldName].AsBsonArray.Count)
                {
                    counter += oneCompareTypeIter;
                }

                c += origDepDocLoop[DBConstants.DependenciesFieldName].AsBsonArray.Count;
            }

            // Goes over all dependencies of original sentence
            // and gets the name of dependency (for example: compound)
            // and checks, if there is, in sentence that is parsed right now,
            // the dependency with same POS tag or same index at governor or dependent.
            oneCompareTypeIter = oneCompareType / (double)(c);
            foreach (BsonDocument origDepDocLoop in dbEntry[DBConstants.OriginalSentenceDependenciesFieldName].AsBsonArray)
            {
                String depName = origDepDocLoop[DBConstants.DependencyNameFieldName].AsString;

                foreach (BsonDocument depLoop in origDepDocLoop[DBConstants.DependenciesFieldName].AsBsonArray)
                {
                    int position = depLoop[DBConstants.PositionFieldName].AsInt32;

                    if (sentence.CompressedDependencies[depName].Where(
                        x => x.Dependent.POS == depLoop[DBConstants.DependentFieldName].AsBsonDocument[DBConstants.POSFieldName]).FirstOrDefault() != null)
                    {
                        counter += oneCompareTypeIter;
                    }
                    else
                    {
                        throw new Exception();
                    }

                    if (sentence.CompressedDependencies[depName].Where(
                        x => x.Dependent.Index == depLoop[DBConstants.DependentFieldName].AsBsonDocument[DBConstants.IndexFieldName]).FirstOrDefault() != null)
                    {
                        counter += oneCompareTypeIter;
                    }
                    else
                    {
                        throw new Exception();
                    }

                    if (sentence.CompressedDependencies[depName].Where(
                        x => x.Governor.POS == depLoop[DBConstants.GovernorFieldName].AsBsonDocument[DBConstants.POSFieldName]).FirstOrDefault() != null)
                    {
                        counter += oneCompareTypeIter;
                    }
                    else
                    {
                        throw new Exception();
                    }

                    if (sentence.CompressedDependencies[depName].Where(
                        x => x.Governor.Index == depLoop[DBConstants.GovernorFieldName].AsBsonDocument[DBConstants.IndexFieldName]).FirstOrDefault() != null)
                    {
                        counter += oneCompareTypeIter;
                    }
                    else
                    {
                        throw new Exception();
                    }

                    if (sentence.CompressedDependencies[depName].Where(
                        x =>x.Governor.POS == depLoop[DBConstants.GovernorFieldName].AsBsonDocument[DBConstants.POSFieldName] 
                        && x.Governor.Index == depLoop[DBConstants.GovernorFieldName].AsBsonDocument[DBConstants.IndexFieldName]).FirstOrDefault() != null)
                    {
                        counter += oneCompareTypeIter;
                    }
                    else
                    {
                        throw new Exception();
                    }

                    if (sentence.CompressedDependencies[depName].Where(
                        x => x.Dependent.POS == depLoop[DBConstants.DependentFieldName].AsBsonDocument[DBConstants.POSFieldName]
                        && x.Dependent.Index == depLoop[DBConstants.DependentFieldName].AsBsonDocument[DBConstants.IndexFieldName]).FirstOrDefault() != null)
                    {
                        counter += oneCompareTypeIter;
                    }
                    else
                    {
                        throw new Exception();
                    }

                    if (sentence.CompressedDependencies[depName].Where(
                        x => x.Dependent.POS == depLoop[DBConstants.DependentFieldName].AsBsonDocument[DBConstants.POSFieldName]
                        && x.Dependent.Index == depLoop[DBConstants.DependentFieldName].AsBsonDocument[DBConstants.IndexFieldName]
                        && x.Governor.POS == depLoop[DBConstants.GovernorFieldName].AsBsonDocument[DBConstants.POSFieldName]
                        && x.Governor.Index == depLoop[DBConstants.GovernorFieldName].AsBsonDocument[DBConstants.IndexFieldName]).FirstOrDefault() != null)
                    {
                        counter += oneCompareTypeIter;
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
            }

            return counter;
        }
    }
}
