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
        public static Structure ParseStructure(BsonDocument persistedStructure)
        {
            String id;
            DateTime createdAt;
            DateTime updatedAt;
            NotenizerDependencies dependencies;

            id = persistedStructure[DBConstants.IdFieldName].AsObjectId.ToString();
            createdAt = persistedStructure[DBConstants.CreatedAtFieldName].ToUniversalTime();
            updatedAt = persistedStructure[DBConstants.UpdatedAtFieldName].ToUniversalTime();
            dependencies = ParseDependencies(persistedStructure, DBConstants.StructureDataFieldName);

            return new Structure(dependencies, id, createdAt, updatedAt);
        }

        public static Sentence ParseSentence(BsonDocument persistedSentnece)
        {
            String id;
            String text;
            String articleID;
            String structureID;
            String ruleID;
            String andRuleID;
            String noteID;
            DateTime createdAt;
            DateTime updatedAt;

            id = persistedSentnece[DBConstants.IdFieldName].AsObjectId.ToString();
            text = persistedSentnece[DBConstants.TextFieldName].AsString;
            articleID = persistedSentnece[DBConstants.ArticleRefIdFieldName].AsObjectId.ToString();
            ruleID = persistedSentnece[DBConstants.RuleRefIdFieldName].AsObjectId.ToString();
            structureID = persistedSentnece[DBConstants.StructureRefIdFieldName].AsObjectId.ToString();
            createdAt = persistedSentnece[DBConstants.CreatedAtFieldName].ToUniversalTime();
            updatedAt = persistedSentnece[DBConstants.UpdatedAtFieldName].ToUniversalTime();
            andRuleID = persistedSentnece[DBConstants.AndRuleRefIdFieldName].ToString();
            noteID = persistedSentnece[DBConstants.NoteRefIdFieldName].AsObjectId.ToString();

            return new Sentence(id, text, articleID, structureID, ruleID, andRuleID, noteID, createdAt, updatedAt);
        }

        public static Article ParseArticle(BsonDocument persistedArticle)
        {
            String id;
            String text;
            DateTime createdAt;
            DateTime updatedAt;

            id = persistedArticle[DBConstants.IdFieldName].AsObjectId.ToString();
            text = persistedArticle[DBConstants.TextFieldName].AsString.Trim();
            createdAt = persistedArticle[DBConstants.CreatedAtFieldName].ToUniversalTime();
            updatedAt = persistedArticle[DBConstants.UpdatedAtFieldName].ToUniversalTime();

            return new Article(id, createdAt, updatedAt, text);
        }

        public static Note ParseNote(BsonDocument persistedNote)
        {
            String id;
            String text;
            String ruleID;
            String andRuleID;
            DateTime createdAt;
            DateTime updatedAt;

            id = persistedNote[DBConstants.IdFieldName].AsObjectId.ToString();
            text = persistedNote[DBConstants.TextFieldName].AsString;
            ruleID = persistedNote[DBConstants.RuleRefIdFieldName].AsObjectId.ToString();
            andRuleID = persistedNote[DBConstants.AndRuleRefIdFieldName].ToString();
            createdAt = persistedNote[DBConstants.CreatedAtFieldName].ToUniversalTime();
            updatedAt = persistedNote[DBConstants.UpdatedAtFieldName].ToUniversalTime();

            return new Note(id, text, ruleID, andRuleID, createdAt, updatedAt);
        }

        public static NotenizerNoteRule ParseRule(BsonDocument persistedRule)
        {
            String id;
            String structureId;
            DateTime createdAt;
            DateTime updatedAt;
            SentencesTerminators sentencesTerminators;

            id = persistedRule[DBConstants.IdFieldName].AsObjectId.ToString();
            structureId = persistedRule[DBConstants.StructureRefIdFieldName].AsObjectId.ToString();
            createdAt = persistedRule[DBConstants.CreatedAtFieldName].ToUniversalTime();
            updatedAt = persistedRule[DBConstants.UpdatedAtFieldName].ToUniversalTime();
            sentencesTerminators = ParseSentencesTerminators(persistedRule[DBConstants.SentenceTerminatorsFieldName].AsBsonArray);

            return new NotenizerNoteRule(id, structureId, createdAt, updatedAt, sentencesTerminators);
        }

        public static NotenizerAndRule ParseAndRule(BsonDocument persistedAndRule)
        {
            String id;
            String structureId;
            DateTime createdAt;
            DateTime updatedAt;
            int setsPosition;
            int sentenceTerminator;

            id = persistedAndRule[DBConstants.IdFieldName].AsObjectId.ToString();
            structureId = persistedAndRule[DBConstants.StructureRefIdFieldName].AsObjectId.ToString();
            createdAt = persistedAndRule[DBConstants.CreatedAtFieldName].ToUniversalTime();
            updatedAt = persistedAndRule[DBConstants.UpdatedAtFieldName].ToUniversalTime();
            setsPosition = persistedAndRule[DBConstants.AndSetPositionFieldName].AsInt32;
            sentenceTerminator = persistedAndRule[DBConstants.SentenceTerminatorsFieldName].AsInt32;

            return new NotenizerAndRule(id, structureId, createdAt, updatedAt, setsPosition, sentenceTerminator);
        }

        public static SentencesTerminators ParseSentencesTerminators(BsonArray persistedSentencesTerminators)
        {
            SentencesTerminators sentencesTerminators = new SentencesTerminators();

            foreach (BsonInt32 terminatorLoop in persistedSentencesTerminators)
                sentencesTerminators.Add((int)terminatorLoop);

            return sentencesTerminators;
        }

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

            SentencesTerminators sentencesEnds = new SentencesTerminators();
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
                    ComparisonType comparisonType = dependencyDocLoop.GetValue(DBConstants.ComparisonTypeFieldName, ComparisonType.Unidentified).AsInt32.ToEnum<ComparisonType>();
                    TokenType tokenType = dependencyDocLoop.GetValue(DBConstants.TokenTypeFieldName, TokenType.Unidentified).AsInt32.ToEnum<TokenType>();

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

        //public static Note ParseNote(BsonDocument dbEntry)
        //{
        //    String id;
        //    String originalSentence;
        //    String note;
        //    DateTime createdAt;
        //    DateTime updatedAt;
        //    CreatedBy createdBy;
        //    String andParserRuleRefId;

        //    id = dbEntry[DBConstants.IdFieldName].AsObjectId.ToString();
        //    originalSentence = dbEntry[DBConstants.OriginalSentenceFieldName].AsString;
        //    note = dbEntry[DBConstants.NoteFieldName].AsString;
        //    createdAt = dbEntry[DBConstants.CreatedAtFieldName].ToUniversalTime();
        //    updatedAt = dbEntry[DBConstants.UpdatedAtFieldName].ToUniversalTime();
        //    createdBy = dbEntry[DBConstants.CreatedByFieldName].AsInt32.ToEnum<CreatedBy>();
        //    andParserRuleRefId = dbEntry[DBConstants.AndRuleRefIdFieldName].ToString();

        //    return new Note(id, originalSentence, note, createdAt, updatedAt, createdBy, andParserRuleRefId);
        //}

        /// <summary>
        /// Gets rule for parsing with the heighest match with original sentence.
        /// </summary>
        /// <param name="sentence"></param>
        /// <param name="dbEntries"></param>
        /// <returns></returns>
        //public static NotenizerNoteRule GetHeighestMatch(NotenizerSentence sentence, List<BsonDocument> dbEntries)
        //{
        //    NotenizerNoteRule rule = null;

        //    foreach (BsonDocument bsonDocLoop in dbEntries)
        //    {
        //        BsonDocument articleDocument = DB.GetFirst(DBConstants.ArticlesCollectionName, DocumentCreator.CreateFilterById(bsonDocLoop[DBConstants.ArticleRefIdFieldName].AsObjectId.ToString())).Result;
        //        BsonDocument ruleDocument = DB.GetFirst(DBConstants.NoteRulesCollectionName, DocumentCreator.CreateFilterById(bsonDocLoop[DBConstants.RuleRefIdFieldName].AsObjectId.ToString())).Result;
        //        NotenizerNoteRule r = ParseNoteRule(ruleDocument);

        //        r.Article = ParseArticle(articleDocument);
        //        r.Note = ParseNote(bsonDocLoop);
        //        //r.Match = CalculateMatch(sentence, r.RuleDependencies, bsonDocLoop);

        //        if (rule == null
        //            || rule.Match.Structure < r.Match.Structure
        //            || (rule.Match.Structure == r.Match.Structure
        //                && (rule.Match.Content < r.Match.Content
        //                    || (rule.CreatedBy == CreatedBy.Notenizer
        //                        && r.CreatedBy == CreatedBy.User))))
        //        {
        //            rule = r;
        //        }
        //    }

        //    return rule;
        //}

        public static Structure GetHeighestMatch(NotenizerStructure structure, List<BsonDocument> persistedStructures, out Match m)
        {
            Structure struc = null;
            Match match;
            Match heighestMatch = null;

            foreach (BsonDocument persistedStructureLoop in persistedStructures)
            {
                match = CalculateMatch(structure, persistedStructureLoop);

                if (heighestMatch == null
                    || heighestMatch.Structure < match.Structure
                    || (heighestMatch.Structure == match.Structure
                        && heighestMatch.Content < match.Content))
                {
                    heighestMatch = match;
                    struc = ParseStructure(persistedStructureLoop);
                }
            }

            m = heighestMatch;
            return struc;
        }

        /// <summary>
        /// Calculates the match between original sentence (from DB) and sentence that is being parsed.
        /// </summary>
        /// <param name="notenizerStructure"></param>
        /// <param name="structure"></param>
        /// <param name="persistedStructure"></param>
        /// <returns></returns>
        private static Match CalculateMatch(NotenizerStructure notenizerStructure, BsonDocument persistedStructure)
        {
            Double structureCompareCount = 5.0;
            Double contentCompareCount = 10.0;
            Double oneStructeCompareRating = NotenizerConstants.MaxMatchValue / structureCompareCount;
            Double oneContentCompareRating = NotenizerConstants.MaxMatchValue / contentCompareCount;
            Double oneStructureCompareTypeIterRating;
            Double oneContentComapareTypeIterRating;
            Double structureCounter = 0.0;
            Double contentCounter = 0.0;
            Double valueCounter = 0.0;
            Dictionary<String, Dictionary<Tuple<PartOfSpeechType, PartOfSpeechType>, int>> structureDic = new Dictionary<string, Dictionary<Tuple<PartOfSpeechType, PartOfSpeechType>, int>>();

            int c = 0;

            oneStructureCompareTypeIterRating = oneStructeCompareRating / Double.Parse(persistedStructure[DBConstants.StructureDataFieldName].AsBsonArray.Count.ToString());
            foreach (BsonDocument origDepDocLoop in persistedStructure[DBConstants.StructureDataFieldName].AsBsonArray)
            {
                String depName = origDepDocLoop[DBConstants.DependencyNameFieldName].AsString;

                if (notenizerStructure.CompressedDependencies[depName].Count == origDepDocLoop[DBConstants.DependenciesFieldName].AsBsonArray.Count)
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
            foreach (BsonDocument origDepDocLoop in persistedStructure[DBConstants.StructureDataFieldName].AsBsonArray)
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

                    if (notenizerStructure.CompressedDependencies[depName].Where(
                        x => x.Dependent.POS.Type == PartOfSpeech.GetTypeFromTag(depLoop[DBConstants.DependentFieldName].AsBsonDocument[DBConstants.POSFieldName].AsString)).FirstOrDefault() != null)
                    {
                        structureCounter += oneStructureCompareTypeIterRating;
                    }

                    if (notenizerStructure.CompressedDependencies[depName].Where(
                        x => x.Governor.POS.Type == PartOfSpeech.GetTypeFromTag(depLoop[DBConstants.GovernorFieldName].AsBsonDocument[DBConstants.POSFieldName].AsString)).FirstOrDefault() != null)
                    {
                        structureCounter += oneStructureCompareTypeIterRating;
                    }

                    if (notenizerStructure.CompressedDependencies[depName].Where(
                        x => x.Dependent.POS.Type == PartOfSpeech.GetTypeFromTag(depLoop[DBConstants.DependentFieldName].AsBsonDocument[DBConstants.POSFieldName].AsString)
                        && x.Governor.POS.Type == PartOfSpeech.GetTypeFromTag(depLoop[DBConstants.GovernorFieldName].AsBsonDocument[DBConstants.POSFieldName].AsString)).FirstOrDefault() != null)
                    {
                        structureCounter += oneStructureCompareTypeIterRating;
                    }


                    /* ================= Content match ================= */

                    if (notenizerStructure.CompressedDependencies[depName].Where(
                        x => x.Dependent.Index == depLoop[DBConstants.DependentFieldName].AsBsonDocument[DBConstants.IndexFieldName]).FirstOrDefault() != null)
                    {
                        contentCounter += oneContentComapareTypeIterRating;
                    }

                    if (notenizerStructure.CompressedDependencies[depName].Where(
                        x => x.Governor.Index == depLoop[DBConstants.GovernorFieldName].AsBsonDocument[DBConstants.IndexFieldName]).FirstOrDefault() != null)
                    {
                        contentCounter += oneContentComapareTypeIterRating;
                    }

                    if (notenizerStructure.CompressedDependencies[depName].Where(
                        x =>x.Governor.POS.Tag == depLoop[DBConstants.GovernorFieldName].AsBsonDocument[DBConstants.POSFieldName] 
                        && x.Governor.Index == depLoop[DBConstants.GovernorFieldName].AsBsonDocument[DBConstants.IndexFieldName]).FirstOrDefault() != null)
                    {
                        contentCounter += oneContentComapareTypeIterRating;
                    }

                    if (notenizerStructure.CompressedDependencies[depName].Where(
                        x => x.Dependent.POS.Tag == depLoop[DBConstants.DependentFieldName].AsBsonDocument[DBConstants.POSFieldName]
                        && x.Dependent.Index == depLoop[DBConstants.DependentFieldName].AsBsonDocument[DBConstants.IndexFieldName]).FirstOrDefault() != null)
                    {
                        contentCounter += oneContentComapareTypeIterRating;
                    }

                    if (notenizerStructure.CompressedDependencies[depName].Where(
                        x => x.Dependent.POS.Tag == depLoop[DBConstants.DependentFieldName].AsBsonDocument[DBConstants.POSFieldName]
                        && x.Dependent.Index == depLoop[DBConstants.DependentFieldName].AsBsonDocument[DBConstants.IndexFieldName]
                        && x.Governor.POS.Tag == depLoop[DBConstants.GovernorFieldName].AsBsonDocument[DBConstants.POSFieldName]
                        && x.Governor.Index == depLoop[DBConstants.GovernorFieldName].AsBsonDocument[DBConstants.IndexFieldName]).FirstOrDefault() != null)
                    {
                        contentCounter += oneContentComapareTypeIterRating;
                    }

                    if (notenizerStructure.CompressedDependencies[depName].Where(
                        x => x.Dependent.NamedEntity.Value == depLoop[DBConstants.DependentFieldName].AsBsonDocument[DBConstants.NERFieldName]).FirstOrDefault() != null)
                    {
                        contentCounter += oneContentComapareTypeIterRating;
                    }

                    if (notenizerStructure.CompressedDependencies[depName].Where(
                        x => x.Governor.NamedEntity.Value == depLoop[DBConstants.GovernorFieldName].AsBsonDocument[DBConstants.NERFieldName]).FirstOrDefault() != null)
                    {
                        contentCounter += oneContentComapareTypeIterRating;
                    }

                    if (notenizerStructure.CompressedDependencies[depName].Where(
                        x => x.Dependent.NamedEntity.Value == depLoop[DBConstants.DependentFieldName].AsBsonDocument[DBConstants.NERFieldName]).FirstOrDefault() != null)
                    {
                        contentCounter += oneContentComapareTypeIterRating;
                    }

                    if (notenizerStructure.CompressedDependencies[depName].Where(
                        x => x.Governor.NamedEntity.Value == depLoop[DBConstants.GovernorFieldName].AsBsonDocument[DBConstants.NERFieldName]).FirstOrDefault() != null)
                    {
                        contentCounter += oneContentComapareTypeIterRating;
                    }

                    if (notenizerStructure.CompressedDependencies[depName].Where(
                        x => x.Dependent.NamedEntity.Value == depLoop[DBConstants.DependentFieldName].AsBsonDocument[DBConstants.NERFieldName]
                        && x.Governor.NamedEntity.Value == depLoop[DBConstants.GovernorFieldName].AsBsonDocument[DBConstants.NERFieldName]).FirstOrDefault() != null)
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
                    if (notenizerStructure.CompressedDependencies[structDicKVPLoop.Key].Where(
                        x => x.Governor.POS.Type == govPOSdepGOVCountKVPLoop.Key.Item1
                        && x.Dependent.POS.Type == govPOSdepGOVCountKVPLoop.Key.Item2).Count() == govPOSdepGOVCountKVPLoop.Value)
                    {
                        y++;
                    }
                }
            }

            structureCounter += oneStructeCompareRating / z * y;

            return new Match(structureCounter, contentCounter, valueCounter);
        }
    }
}
