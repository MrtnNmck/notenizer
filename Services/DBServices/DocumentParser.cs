using MongoDB.Bson;
using nsConstants;
using nsDB;
using nsEnums;
using nsExtensions;
using nsNotenizerObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace nsServices.DBServices
{
    public static class DocumentParser
    {
        /// <summary>
        /// Makes rule for parsing the sentence from entry from database.
        /// </summary>
        /// <param name="dbEntry">Entry from database</param>
        /// <returns></returns>
        public static NotenizerNoteRule ParseNoteRule(BsonDocument dbEntry)
        {
            NotenizerDependencies dependencies;

            CreatedBy createdBy = dbEntry[DBConstants.CreatedByFieldName].AsInt32.ToEnum<CreatedBy>();
            String _id = dbEntry[DBConstants.IdFieldName].AsObjectId.ToString();

            dependencies = ParseDependencies(dbEntry, DBConstants.NoteDependenciesFieldName);

            List<int> sentencesEnds = new List<int>();
            foreach (BsonInt32 endLoop in dbEntry[DBConstants.SentenceTerminatorsFieldName].AsBsonArray)
                sentencesEnds.Add((int)endLoop);

            return new NotenizerNoteRule(_id, dependencies, sentencesEnds, createdBy);
        }

        public static NotenizerAndRule ParseAndParserRule(BsonDocument dbEntry)
        {
            NotenizerDependencies dependencies;
            int sentenceEnd;
            CreatedBy createdBy;
            int setsPosition;
            String id;

            createdBy = dbEntry[DBConstants.CreatedByFieldName].AsInt32.ToEnum<CreatedBy>();
            id = dbEntry[DBConstants.IdFieldName].AsObjectId.ToString();
            dependencies = ParseDependencies(dbEntry, DBConstants.NoteDependenciesFieldName);
            setsPosition = dbEntry[DBConstants.AndSetPositionFieldName].AsInt32;
            sentenceEnd = dbEntry[DBConstants.SentenceTerminatorsFieldName].AsInt32;

            return new NotenizerAndRule(id, dependencies, createdBy, setsPosition, sentenceEnd);
        }

        private static NotenizerDependencies ParseDependencies(BsonDocument dbEntry, String noteFieldName)
        {
            NotenizerDependencies dependencies = new NotenizerDependencies();

            // foreach note dependency
            foreach (BsonDocument documentLoop in dbEntry[noteFieldName].AsBsonArray)
            {
                NotenizerRelation relation = new NotenizerRelation(documentLoop[DBConstants.DependencyNameFieldName].AsString);

                // foreach dependency in array of dependencies with same relation name
                foreach (BsonDocument dependencyDocLoop in documentLoop[DBConstants.DependenciesFieldName].AsBsonArray)
                {
                    BsonDocument governorDoc = dependencyDocLoop[DBConstants.GovernorFieldName].AsBsonDocument;
                    BsonDocument dependantDoc = dependencyDocLoop[DBConstants.DependentFieldName].AsBsonDocument;
                    int position = dependencyDocLoop[DBConstants.PositionFieldName].AsInt32;
                    ComparisonType comparisonType = dependencyDocLoop[DBConstants.ComparisonTypeFieldName].AsInt32.ToEnum<ComparisonType>();
                    TokenType tokenType = dependencyDocLoop[DBConstants.TokenTypeFieldName].AsInt32.ToEnum<TokenType>();

                    NotenizerWord governor = new NotenizerWord(
                        governorDoc[DBConstants.POSFieldName].AsString, 
                        governorDoc[DBConstants.IndexFieldName].AsInt32,
                        governorDoc[DBConstants.LemmaFieldName].AsString,
                        governorDoc[DBConstants.NERFieldName].AsString);

                    NotenizerWord dependent = new NotenizerWord(
                        dependantDoc[DBConstants.POSFieldName].AsString, 
                        dependantDoc[DBConstants.IndexFieldName].AsInt32,
                        dependantDoc[DBConstants.LemmaFieldName].AsString,
                        dependantDoc[DBConstants.NERFieldName].AsString);

                    NotenizerDependency dependency = new NotenizerDependency(governor, dependent, relation, position, comparisonType, tokenType);

                    dependencies.Add(dependency);
                }
            }

            return dependencies;
        }

        public static Note ParseNote(BsonDocument dbEntry)
        {
            String id;
            String originalSentence;
            String note;
            DateTime createdAt;
            DateTime updatedAt;
            CreatedBy createdBy;
            String andParserRuleRefId;

            id = dbEntry[DBConstants.IdFieldName].AsObjectId.ToString();
            originalSentence = dbEntry[DBConstants.OriginalSentenceFieldName].AsString;
            note = dbEntry[DBConstants.NoteFieldName].AsString;
            createdAt = dbEntry[DBConstants.CreatedAtFieldName].ToUniversalTime();
            updatedAt = dbEntry[DBConstants.UpdatedAtFieldName].ToUniversalTime();
            createdBy = dbEntry[DBConstants.CreatedByFieldName].AsInt32.ToEnum<CreatedBy>();
            andParserRuleRefId = dbEntry[DBConstants.AndRuleRefIdFieldName].ToString();

            return new Note(id, originalSentence, note, createdAt, updatedAt, createdBy, andParserRuleRefId);
        }

        public static Article ParseArticle(BsonDocument dbEntry)
        {
            String id;
            String article;
            DateTime createdAt;
            DateTime updatedAt;

            id = dbEntry[DBConstants.IdFieldName].AsObjectId.ToString();
            createdAt = dbEntry[DBConstants.CreatedAtFieldName].ToUniversalTime();
            updatedAt = dbEntry[DBConstants.UpdatedAtFieldName].ToUniversalTime();
            article = dbEntry[DBConstants.TextFieldName].ToString().Trim();

            return new Article(id, createdAt, updatedAt, article);
        }

        /// <summary>
        /// Gets rule for parsing with the heighest match with original sentence.
        /// </summary>
        /// <param name="sentence"></param>
        /// <param name="dbEntries"></param>
        /// <returns></returns>
        public static NotenizerNoteRule GetHeighestMatch(NotenizerSentence sentence, List<BsonDocument> dbEntries)
        {
            NotenizerNoteRule rule = null;

            foreach (BsonDocument bsonDocLoop in dbEntries)
            {
                BsonDocument articleDocument = DB.GetFirst(DBConstants.ArticlesCollectionName, DocumentCreator.CreateFilterById(bsonDocLoop[DBConstants.ArticleRefIdFieldName].AsObjectId.ToString())).Result;
                BsonDocument ruleDocument = DB.GetFirst(DBConstants.NoteRulesCollectionName, DocumentCreator.CreateFilterById(bsonDocLoop[DBConstants.RuleRefIdFieldName].AsObjectId.ToString())).Result;
                NotenizerNoteRule r = ParseNoteRule(ruleDocument);

                r.Article = ParseArticle(articleDocument);
                r.Note = ParseNote(bsonDocLoop);
                r.Match = CalculateMatch(sentence, r.RuleDependencies, bsonDocLoop);

                if (rule == null
                    || rule.Match.Structure < r.Match.Structure
                    || (rule.Match.Structure == r.Match.Structure
                        && (rule.Match.Content < r.Match.Content
                            || (rule.CreatedBy == CreatedBy.Notenizer
                                && r.CreatedBy == CreatedBy.User))))
                {
                    rule = r;
                }
            }

            return rule;
        }

        /// <summary>
        /// Calculates the match between original sentence (from DB) and sentence that is being parsed.
        /// </summary>
        /// <param name="sentence"></param>
        /// <param name="parsedDependencies"></param>
        /// <param name="dbEntry"></param>
        /// <returns></returns>
        private static Match CalculateMatch(NotenizerSentence sentence, List<NotenizerDependency> parsedDependencies, BsonDocument dbEntry)
        {
            Double structureCompareCount = 5.0;
            Double contentCompareCount = 12.0;
            Double oneStructeCompareRating = NotenizerConstants.MaximumMatchPercentageValue / structureCompareCount;
            Double oneContentCompareRating = NotenizerConstants.MaximumMatchPercentageValue / contentCompareCount;
            Double oneStructureCompareTypeIterRating;
            Double oneContentComapareTypeIterRating;
            Double structureCounter = 0.0;
            Double contentCounter = 0.0;
            Dictionary<String, Dictionary<Tuple<PartOfSpeechType, PartOfSpeechType>, int>> structureDic = new Dictionary<string, Dictionary<Tuple<PartOfSpeechType, PartOfSpeechType>, int>>();

            int c = 0;

            oneStructureCompareTypeIterRating = oneStructeCompareRating / Double.Parse(dbEntry[DBConstants.OriginalSentenceDependenciesFieldName].AsBsonArray.Count.ToString());
            foreach (BsonDocument origDepDocLoop in dbEntry[DBConstants.OriginalSentenceDependenciesFieldName].AsBsonArray)
            {
                String depName = origDepDocLoop[DBConstants.DependencyNameFieldName].AsString;

                if (sentence.CompressedDependencies[depName].Count == origDepDocLoop[DBConstants.DependenciesFieldName].AsBsonArray.Count)
                {
                    structureCounter += oneStructureCompareTypeIterRating;
                }

                c += origDepDocLoop[DBConstants.DependenciesFieldName].AsBsonArray.Count;
            }

            // Goes over all dependencies of original sentence
            // and gets the name of dependency (for example: compound)
            // and checks, if there is, in sentence that is parsed right now,
            // the dependency with same POS tag or same index at governor or dependent.
            oneStructureCompareTypeIterRating = oneStructeCompareRating / (double)(c);
            oneContentComapareTypeIterRating = oneContentCompareRating / (double)(c);
            foreach (BsonDocument origDepDocLoop in dbEntry[DBConstants.OriginalSentenceDependenciesFieldName].AsBsonArray)
            {
                String depName = origDepDocLoop[DBConstants.DependencyNameFieldName].AsString;

                foreach (BsonDocument depLoop in origDepDocLoop[DBConstants.DependenciesFieldName].AsBsonArray)
                {
                    /* ================= Structure match ================= */
                    if (!structureDic.ContainsKey(depName))
                        structureDic.Add(depName, new Dictionary<Tuple<PartOfSpeechType, PartOfSpeechType>, int>());

                    Tuple<PartOfSpeechType, PartOfSpeechType> govPOSdepPOSKey = new Tuple<PartOfSpeechType, PartOfSpeechType>(
                        PartOfSpeech.GetTypeFromTag(depLoop[DBConstants.GovernorFieldName].AsBsonDocument[DBConstants.POSFieldName].AsString),
                        PartOfSpeech.GetTypeFromTag(depLoop[DBConstants.DependentFieldName].AsBsonDocument[DBConstants.POSFieldName].AsString));

                    if (!structureDic[depName].ContainsKey(govPOSdepPOSKey))
                        structureDic[depName].Add(govPOSdepPOSKey, 0);

                    structureDic[depName][govPOSdepPOSKey]++;

                    if (sentence.CompressedDependencies[depName].Where(
                        x => x.Dependent.POS.Type == PartOfSpeech.GetTypeFromTag(depLoop[DBConstants.DependentFieldName].AsBsonDocument[DBConstants.POSFieldName].AsString)).FirstOrDefault() != null)
                    {
                        structureCounter += oneStructureCompareTypeIterRating;
                    }

                    if (sentence.CompressedDependencies[depName].Where(
                        x => x.Governor.POS.Type == PartOfSpeech.GetTypeFromTag(depLoop[DBConstants.GovernorFieldName].AsBsonDocument[DBConstants.POSFieldName].AsString)).FirstOrDefault() != null)
                    {
                        structureCounter += oneStructureCompareTypeIterRating;
                    }

                    if (sentence.CompressedDependencies[depName].Where(
                        x => x.Dependent.POS.Type == PartOfSpeech.GetTypeFromTag(depLoop[DBConstants.DependentFieldName].AsBsonDocument[DBConstants.POSFieldName].AsString)
                        && x.Governor.POS.Type == PartOfSpeech.GetTypeFromTag(depLoop[DBConstants.GovernorFieldName].AsBsonDocument[DBConstants.POSFieldName].AsString)).FirstOrDefault() != null)
                    {
                        structureCounter += oneStructureCompareTypeIterRating;
                    }


                    /* ================= Content match ================= */

                    if (sentence.CompressedDependencies[depName].Where(
                        x => x.Dependent.Index == depLoop[DBConstants.DependentFieldName].AsBsonDocument[DBConstants.IndexFieldName]).FirstOrDefault() != null)
                    {
                        contentCounter += oneContentComapareTypeIterRating;
                    }

                    if (sentence.CompressedDependencies[depName].Where(
                        x => x.Governor.Index == depLoop[DBConstants.GovernorFieldName].AsBsonDocument[DBConstants.IndexFieldName]).FirstOrDefault() != null)
                    {
                        contentCounter += oneContentComapareTypeIterRating;
                    }

                    if (sentence.CompressedDependencies[depName].Where(
                        x =>x.Governor.POS.Tag == depLoop[DBConstants.GovernorFieldName].AsBsonDocument[DBConstants.POSFieldName] 
                        && x.Governor.Index == depLoop[DBConstants.GovernorFieldName].AsBsonDocument[DBConstants.IndexFieldName]).FirstOrDefault() != null)
                    {
                        contentCounter += oneContentComapareTypeIterRating;
                    }

                    if (sentence.CompressedDependencies[depName].Where(
                        x => x.Dependent.POS.Tag == depLoop[DBConstants.DependentFieldName].AsBsonDocument[DBConstants.POSFieldName]
                        && x.Dependent.Index == depLoop[DBConstants.DependentFieldName].AsBsonDocument[DBConstants.IndexFieldName]).FirstOrDefault() != null)
                    {
                        contentCounter += oneContentComapareTypeIterRating;
                    }

                    if (sentence.CompressedDependencies[depName].Where(
                        x => x.Dependent.POS.Tag == depLoop[DBConstants.DependentFieldName].AsBsonDocument[DBConstants.POSFieldName]
                        && x.Dependent.Index == depLoop[DBConstants.DependentFieldName].AsBsonDocument[DBConstants.IndexFieldName]
                        && x.Governor.POS.Tag == depLoop[DBConstants.GovernorFieldName].AsBsonDocument[DBConstants.POSFieldName]
                        && x.Governor.Index == depLoop[DBConstants.GovernorFieldName].AsBsonDocument[DBConstants.IndexFieldName]).FirstOrDefault() != null)
                    {
                        contentCounter += oneContentComapareTypeIterRating;
                    }

                    if (sentence.CompressedDependencies[depName].Where(
                        x => x.Dependent.NamedEntity.Value == depLoop[DBConstants.DependentFieldName].AsBsonDocument[DBConstants.NERFieldName]).FirstOrDefault() != null)
                    {
                        contentCounter += oneContentComapareTypeIterRating;
                    }

                    if (sentence.CompressedDependencies[depName].Where(
                        x => x.Dependent.Lemma == depLoop[DBConstants.DependentFieldName].AsBsonDocument[DBConstants.LemmaFieldName]).FirstOrDefault() != null)
                    {
                        contentCounter += oneContentComapareTypeIterRating;
                    }

                    if (sentence.CompressedDependencies[depName].Where(
                        x => x.Governor.NamedEntity.Value == depLoop[DBConstants.GovernorFieldName].AsBsonDocument[DBConstants.NERFieldName]).FirstOrDefault() != null)
                    {
                        contentCounter += oneContentComapareTypeIterRating;
                    }

                    if (sentence.CompressedDependencies[depName].Where(
                        x => x.Governor.Lemma == depLoop[DBConstants.GovernorFieldName].AsBsonDocument[DBConstants.LemmaFieldName]).FirstOrDefault() != null)
                    {
                        contentCounter += oneContentComapareTypeIterRating;
                    }

                    if (sentence.CompressedDependencies[depName].Where(
                        x => x.Dependent.NamedEntity.Value == depLoop[DBConstants.DependentFieldName].AsBsonDocument[DBConstants.NERFieldName]
                        && x.Dependent.Lemma == depLoop[DBConstants.DependentFieldName].AsBsonDocument[DBConstants.LemmaFieldName]).FirstOrDefault() != null)
                    {
                        contentCounter += oneContentComapareTypeIterRating;
                    }

                    if (sentence.CompressedDependencies[depName].Where(
                        x => x.Governor.NamedEntity.Value == depLoop[DBConstants.GovernorFieldName].AsBsonDocument[DBConstants.NERFieldName]
                        && x.Governor.Lemma == depLoop[DBConstants.GovernorFieldName].AsBsonDocument[DBConstants.LemmaFieldName]).FirstOrDefault() != null)
                    {
                        contentCounter += oneContentComapareTypeIterRating;
                    }

                    if (sentence.CompressedDependencies[depName].Where(
                        x => x.Dependent.NamedEntity.Value == depLoop[DBConstants.DependentFieldName].AsBsonDocument[DBConstants.NERFieldName]
                        && x.Dependent.Lemma == depLoop[DBConstants.DependentFieldName].AsBsonDocument[DBConstants.LemmaFieldName]
                        && x.Governor.NamedEntity.Value == depLoop[DBConstants.GovernorFieldName].AsBsonDocument[DBConstants.NERFieldName]
                        && x.Governor.Lemma == depLoop[DBConstants.GovernorFieldName].AsBsonDocument[DBConstants.LemmaFieldName]).FirstOrDefault() != null)
                    {
                        contentCounter += oneContentComapareTypeIterRating;
                    }
                }
            }

            int z = 0;
            int y = 0;
            foreach (KeyValuePair<String, Dictionary<Tuple<PartOfSpeechType, PartOfSpeechType>, int>> structDicKVPLoop in structureDic)
            {
                z += structDicKVPLoop.Value.Keys.Count;

                foreach (KeyValuePair<Tuple<PartOfSpeechType, PartOfSpeechType>, int> govPOSdepGOVCountKVPLoop in structDicKVPLoop.Value)
                {
                    if (sentence.CompressedDependencies[structDicKVPLoop.Key].Where(
                        x => x.Governor.POS.Type == govPOSdepGOVCountKVPLoop.Key.Item1
                        && x.Dependent.POS.Type == govPOSdepGOVCountKVPLoop.Key.Item2).Count() == govPOSdepGOVCountKVPLoop.Value)
                    {
                        y++;
                    }
                }
            }

            structureCounter += oneStructeCompareRating / z * y;

            return new Match(structureCounter, contentCounter);
        }
    }
}
