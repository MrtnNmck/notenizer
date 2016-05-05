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
        #region Properties

        #endregion Properties

        #region Methods

        /// <summary>
        /// Create structure from document.
        /// </summary>
        /// <param name="persistedStructure"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Creates sentence from document.
        /// </summary>
        /// <param name="persistedSentnece"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Creates article from document.
        /// </summary>
        /// <param name="persistedArticle"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Creates note from document.
        /// </summary>
        /// <param name="persistedNote"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Creates rule from document.
        /// </summary>
        /// <param name="persistedRule"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Creates And-Rule from deocument.
        /// </summary>
        /// <param name="persistedAndRule"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets sentence terminators from array.
        /// </summary>
        /// <param name="persistedSentencesTerminators"></param>
        /// <returns></returns>
        public static SentencesTerminators ParseSentencesTerminators(BsonArray persistedSentencesTerminators)
        {
            SentencesTerminators sentencesTerminators = new SentencesTerminators();

            foreach (BsonInt32 terminatorLoop in persistedSentencesTerminators)
                sentencesTerminators.Add((int)terminatorLoop);

            return sentencesTerminators;
        }

        /// <summary>
        /// Create dependencies from deocument.
        /// </summary>
        /// <param name="dbEntry"></param>
        /// <param name="noteFieldName"></param>
        /// <returns></returns>
        private static NotenizerDependencies ParseDependencies(BsonDocument dbEntry, String noteFieldName)
        {
            int position;
            TokenType tokenType;
            NotenizerWord governor;
            NotenizerWord dependent;
            BsonDocument governorDoc;
            BsonDocument dependantDoc;
            NotenizerRelation relation;
            ComparisonType comparisonType;
            NotenizerDependency dependency;
            NotenizerDependencies dependencies = new NotenizerDependencies();
            // foreach note dependency
            foreach (BsonDocument documentLoop in dbEntry[noteFieldName].AsBsonArray)
            {
                relation = new NotenizerRelation(documentLoop[DBConstants.RelationNameFieldName].AsString);

                // foreach dependency in array of dependencies with same relation name
                foreach (BsonDocument dependencyDocLoop in documentLoop[DBConstants.DependenciesFieldName].AsBsonArray)
                {
                    governorDoc = dependencyDocLoop[DBConstants.GovernorFieldName].AsBsonDocument;
                    dependantDoc = dependencyDocLoop[DBConstants.DependentFieldName].AsBsonDocument;
                    position = dependencyDocLoop[DBConstants.PositionFieldName].AsInt32;
                    comparisonType = dependencyDocLoop.GetValue(DBConstants.ComparisonTypeFieldName, ComparisonType.Unidentified).AsInt32.ToEnum<ComparisonType>();
                    tokenType = dependencyDocLoop.GetValue(DBConstants.TokenTypeFieldName, TokenType.Unidentified).AsInt32.ToEnum<TokenType>();

                    governor = new NotenizerWord(
                        governorDoc[DBConstants.POSFieldName].AsString, 
                        governorDoc[DBConstants.IndexFieldName].AsInt32,
                        governorDoc[DBConstants.LemmaFieldName].AsString,
                        governorDoc[DBConstants.NERFieldName].AsString);

                    dependent = new NotenizerWord(
                        dependantDoc[DBConstants.POSFieldName].AsString, 
                        dependantDoc[DBConstants.IndexFieldName].AsInt32,
                        dependantDoc[DBConstants.LemmaFieldName].AsString,
                        dependantDoc[DBConstants.NERFieldName].AsString);

                    dependency = new NotenizerDependency(governor, dependent, relation, position, comparisonType, tokenType);

                    dependencies.Add(dependency);
                }
            }

            return dependencies;
        }

        /// <summary>
        /// Gets structure of heighest match.
        /// </summary>
        /// <param name="structure"></param>
        /// <param name="persistedStructures"></param>
        /// <param name="m"></param>
        /// <returns></returns>
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
            Double contentCompareCount = 8.0;
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
                String relationName = origDepDocLoop[DBConstants.RelationNameFieldName].AsString;

                if (notenizerStructure.CompressedDependencies[relationName].Count == origDepDocLoop[DBConstants.DependenciesFieldName].AsBsonArray.Count)
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
                String relationName = origDepDocLoop[DBConstants.RelationNameFieldName].AsString;

                foreach (BsonDocument depLoop in origDepDocLoop[DBConstants.DependenciesFieldName].AsBsonArray)
                {
                    /* ================= Structure match ================= */
                    if (!structureDic.ContainsKey(relationName))
                        structureDic.Add(relationName, new Dictionary<Tuple<PartOfSpeechType, PartOfSpeechType>, int>());

                    Tuple<PartOfSpeechType, PartOfSpeechType> govPOSdepPOSKey = new Tuple<PartOfSpeechType, PartOfSpeechType>(
                        PartOfSpeech.GetTypeFromTag(depLoop[DBConstants.GovernorFieldName].AsBsonDocument[DBConstants.POSFieldName].AsString),
                        PartOfSpeech.GetTypeFromTag(depLoop[DBConstants.DependentFieldName].AsBsonDocument[DBConstants.POSFieldName].AsString));

                    if (!structureDic[relationName].ContainsKey(govPOSdepPOSKey))
                        structureDic[relationName].Add(govPOSdepPOSKey, 0);

                    structureDic[relationName][govPOSdepPOSKey]++;

                    if (notenizerStructure.CompressedDependencies[relationName].Where(
                        x => x.Dependent.POS.Type == PartOfSpeech.GetTypeFromTag(depLoop[DBConstants.DependentFieldName].AsBsonDocument[DBConstants.POSFieldName].AsString)).FirstOrDefault() != null)
                    {
                        structureCounter += oneStructureCompareTypeIterRating;
                    }

                    if (notenizerStructure.CompressedDependencies[relationName].Where(
                        x => x.Governor.POS.Type == PartOfSpeech.GetTypeFromTag(depLoop[DBConstants.GovernorFieldName].AsBsonDocument[DBConstants.POSFieldName].AsString)).FirstOrDefault() != null)
                    {
                        structureCounter += oneStructureCompareTypeIterRating;
                    }

                    if (notenizerStructure.CompressedDependencies[relationName].Where(
                        x => x.Dependent.POS.Type == PartOfSpeech.GetTypeFromTag(depLoop[DBConstants.DependentFieldName].AsBsonDocument[DBConstants.POSFieldName].AsString)
                        && x.Governor.POS.Type == PartOfSpeech.GetTypeFromTag(depLoop[DBConstants.GovernorFieldName].AsBsonDocument[DBConstants.POSFieldName].AsString)).FirstOrDefault() != null)
                    {
                        structureCounter += oneStructureCompareTypeIterRating;
                    }


                    /* ================= Content match ================= */

                    if (notenizerStructure.CompressedDependencies[relationName].Where(
                        x => x.Dependent.Index == depLoop[DBConstants.DependentFieldName].AsBsonDocument[DBConstants.IndexFieldName]).FirstOrDefault() != null)
                    {
                        contentCounter += oneContentComapareTypeIterRating;
                    }

                    if (notenizerStructure.CompressedDependencies[relationName].Where(
                        x => x.Governor.Index == depLoop[DBConstants.GovernorFieldName].AsBsonDocument[DBConstants.IndexFieldName]).FirstOrDefault() != null)
                    {
                        contentCounter += oneContentComapareTypeIterRating;
                    }

                    if (notenizerStructure.CompressedDependencies[relationName].Where(
                        x =>x.Governor.POS.Tag == depLoop[DBConstants.GovernorFieldName].AsBsonDocument[DBConstants.POSFieldName] 
                        && x.Governor.Index == depLoop[DBConstants.GovernorFieldName].AsBsonDocument[DBConstants.IndexFieldName]).FirstOrDefault() != null)
                    {
                        contentCounter += oneContentComapareTypeIterRating;
                    }

                    if (notenizerStructure.CompressedDependencies[relationName].Where(
                        x => x.Dependent.POS.Tag == depLoop[DBConstants.DependentFieldName].AsBsonDocument[DBConstants.POSFieldName]
                        && x.Dependent.Index == depLoop[DBConstants.DependentFieldName].AsBsonDocument[DBConstants.IndexFieldName]).FirstOrDefault() != null)
                    {
                        contentCounter += oneContentComapareTypeIterRating;
                    }

                    if (notenizerStructure.CompressedDependencies[relationName].Where(
                        x => x.Dependent.POS.Tag == depLoop[DBConstants.DependentFieldName].AsBsonDocument[DBConstants.POSFieldName]
                        && x.Dependent.Index == depLoop[DBConstants.DependentFieldName].AsBsonDocument[DBConstants.IndexFieldName]
                        && x.Governor.POS.Tag == depLoop[DBConstants.GovernorFieldName].AsBsonDocument[DBConstants.POSFieldName]
                        && x.Governor.Index == depLoop[DBConstants.GovernorFieldName].AsBsonDocument[DBConstants.IndexFieldName]).FirstOrDefault() != null)
                    {
                        contentCounter += oneContentComapareTypeIterRating;
                    }

                    if (notenizerStructure.CompressedDependencies[relationName].Where(
                        x => x.Dependent.NamedEntity.Value == depLoop[DBConstants.DependentFieldName].AsBsonDocument[DBConstants.NERFieldName]).FirstOrDefault() != null)
                    {
                        contentCounter += oneContentComapareTypeIterRating;
                    }

                    if (notenizerStructure.CompressedDependencies[relationName].Where(
                        x => x.Governor.NamedEntity.Value == depLoop[DBConstants.GovernorFieldName].AsBsonDocument[DBConstants.NERFieldName]).FirstOrDefault() != null)
                    {
                        contentCounter += oneContentComapareTypeIterRating;
                    }

                    if (notenizerStructure.CompressedDependencies[relationName].Where(
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

        #endregion Methods
    }
}
