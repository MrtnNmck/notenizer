using MongoDB.Bson;
using MongoDB.Driver;
using nsConstants;
using nsEnums;
using nsNotenizerObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsNotenizer
{
    public static class DocumentCreator
    {
        public static BsonDocument CreateNoteDocument(NotenizerNote note, String articleId, String noteRuleId, String andParseRuleId)
        {
            BsonDocument doc = new BsonDocument();
            BsonValue articleObjectId;
            BsonValue andParserRuleObjectId;
            BsonArray originalDepencenciesArr = new BsonArray();
            Dictionary<String, BsonArray> dependencies = new Dictionary<String, BsonArray>();

            if (articleId == String.Empty)
                articleObjectId = BsonNull.Value;
            else
                articleObjectId = ObjectId.Parse(articleId);

            if (andParseRuleId == String.Empty)
                andParserRuleObjectId = BsonNull.Value;
            else
                andParserRuleObjectId = ObjectId.Parse(andParseRuleId);

            doc.Add(DBConstants.OriginalSentenceFieldName, new BsonString(note.OriginalSentence.ToString()));
            doc.Add(DBConstants.NoteFieldName, new BsonString(note.Value));
            doc.Add(DBConstants.OriginalSentenceDependenciesFieldName, originalDepencenciesArr);
            doc.Add(DBConstants.ArticleIdFieldName, articleObjectId);
            doc.Add(DBConstants.NoteRuleRefIdFieldName, ObjectId.Parse(noteRuleId));
            doc.Add(DBConstants.AndParserRuleRefIdFieldName, andParserRuleObjectId);
            doc.Add(DBConstants.CreatedByFieldName, new BsonInt32((int)note.CreatedBy));
            doc.Add(DBConstants.CreatedAtFieldName, new BsonDateTime(note.CreatedAt));
            doc.Add(DBConstants.UpdatedAtFieldName, new BsonDateTime(note.UpdatedAt));

            foreach (NotenizerDependency dependencyLoop in note.OriginalSentence.Dependencies)
            {
                BsonDocument dependencyDoc = CreateDependencyDocument(dependencyLoop);

                AppendDependencyDocument(dependencyLoop, dependencyDoc, originalDepencenciesArr, dependencies);
            }

            return doc;
        }

        public static BsonDocument CreateNoteRuleDocument(NotenizerNote note)
        {
            return CreateNoteRuleDocument(DateTime.Now, DateTime.Now, CreatedBy.Notenizer, note);
        }

        public static BsonDocument CreateNoteRuleDocument(NotenizerNoteRule rule, NotenizerNote note)
        {
            return CreateNoteRuleDocument(rule.CreatedAt, rule.UpdatedAt, rule.CreatedBy, note);
        }

        public static BsonDocument CreateNoteRuleDocument(DateTime createdAt, DateTime updatedAt, CreatedBy createdBy, NotenizerNote note)
        {
            BsonDocument doc = new BsonDocument();
            BsonArray sentencesEnds = new BsonArray();

            doc.Add(DBConstants.CreatedByFieldName, new BsonInt32((int)createdBy));
            doc.Add(DBConstants.CreatedAtFieldName, new BsonDateTime(createdAt));
            doc.Add(DBConstants.UpdatedAtFieldName, new BsonDateTime(updatedAt));
            doc.Add(DBConstants.SentencesEndsFieldName, sentencesEnds);

            BsonArray noteDependenciesArr = CreateNoteRuleNotDependenciesArray(note, sentencesEnds);

            doc.Add(DBConstants.NoteDependenciesFieldName, noteDependenciesArr);

            return doc;
        }

        public static BsonDocument CreateAndParserRuleDocument(NotenizerNote note, List<NotePart> andParserRuleNoteParts, int setsPosition)
        {
            BsonDocument doc = new BsonDocument();
            Dictionary<String, BsonArray> dependencies = new Dictionary<String, BsonArray>();

            doc.Add(DBConstants.CreatedByFieldName, new BsonInt32((int)note.CreatedBy));
            doc.Add(DBConstants.CreatedAtFieldName, new BsonDateTime(note.CreatedAt));
            doc.Add(DBConstants.UpdatedAtFieldName, new BsonDateTime(note.UpdatedAt));
            doc.Add(DBConstants.AndSetsPositionsFieldName, new BsonInt32(setsPosition));

            BsonArray noteDependenciesArr = CreateNoteRuleNotDependenciesArray(andParserRuleNoteParts);

            doc.Add(DBConstants.NoteDependenciesFieldName, noteDependenciesArr);

            return doc;
        }

        public static BsonDocument CreateNoteDocument(NotenizerNote note, int articleId, List<int> andSetsPositions)
        {
            BsonDocument doc = CreateNoteDocument(note, articleId);

            doc[DBConstants.AdditionalInformationFieldName][DBConstants.AndSetsPositionsFieldName] = new BsonArray(andSetsPositions);

            return doc;
        }

        public static BsonDocument CreateNoteDocument(NotenizerNote note, int articleId)
        {
            int dependencyPosition = 0;
            Dictionary<String, BsonArray> dependencies = new Dictionary<String, BsonArray>();

            BsonDocument doc = new BsonDocument();
            BsonDocument additionInformationDoc = new BsonDocument();
            BsonArray sentencesEnds = new BsonArray();
            BsonArray andSetsPositions = new BsonArray();

            doc.Add(DBConstants.OriginalSentenceFieldName, new BsonString(note.OriginalSentence.ToString()));
            doc.Add(DBConstants.NoteFieldName, new BsonString(note.Value));
            doc.Add(DBConstants.CreatedByFieldName, new BsonInt32((int)note.CreatedBy));
            doc.Add(DBConstants.ArticleIdFieldName, new BsonInt32(articleId));
            doc.Add(DBConstants.CreatedAtFieldName, new BsonDateTime(note.CreatedAt));
            doc.Add(DBConstants.UpdatedAtFieldName, new BsonDateTime(note.UpdatedAt));
            doc.Add(DBConstants.AdditionalInformationFieldName, additionInformationDoc);
            additionInformationDoc.Add(DBConstants.SentencesEndsFieldName, sentencesEnds);
            additionInformationDoc.Add(DBConstants.AndSetsPositionsFieldName, andSetsPositions);

            BsonArray originalDepencenciesArr = new BsonArray();
            foreach (NotenizerDependency dependencyLoop in note.OriginalSentence.Dependencies)
            {
                BsonDocument dependencyDoc = CreateDependencyDocument(dependencyLoop);

                AppendDependencyDocument(dependencyLoop, dependencyDoc, originalDepencenciesArr, dependencies);

                dependencyPosition++;
            }

            doc.Add(DBConstants.OriginalSentenceDependenciesFieldName, originalDepencenciesArr);

            BsonArray noteDependenciesArr = CreateNoteRuleNotDependenciesArray(note, sentencesEnds);

            doc.Add(DBConstants.NoteDependenciesFieldName, noteDependenciesArr);
            return doc;
        }

        public static BsonArray CreateNoteRuleNotDependenciesArray(List<NotePart> noteParts)
        {
            BsonArray temp = new BsonArray();

            return CreateNoteRuleNotDependenciesArray(noteParts, temp);
        }

        public static BsonArray CreateNoteRuleNotDependenciesArray(NotenizerNote note, BsonArray sentencesEnds)
        {
            return CreateNoteRuleNotDependenciesArray(note.NoteParts, sentencesEnds);
        }

        private static BsonArray CreateNoteRuleNotDependenciesArray(List<NotePart> noteParts, BsonArray sentencesEnds)
        {
            int dependencyPosition = 0;
            BsonArray noteDependenciesArr = new BsonArray();
            Dictionary<String, BsonArray> dependencies = new Dictionary<String, BsonArray>();

            foreach (NotePart notePartLoop in noteParts)
            {
                foreach (NoteParticle noteObjectLoop in notePartLoop.NoteParticles)
                {
                    if (noteObjectLoop == null)
                        continue;

                    BsonDocument dependencyDoc = CreateDependencyDocument(noteObjectLoop.NoteDependency, noteObjectLoop.NoteDependency.ComparisonType, noteObjectLoop.NoteDependency.TokenType);

                    AppendDependencyDocument(noteObjectLoop.NoteDependency, dependencyDoc, noteDependenciesArr, dependencies);

                    dependencyPosition++;
                }

                sentencesEnds.Add(new BsonInt32(dependencyPosition));
            }

            return noteDependenciesArr;
        }

        private static BsonDocument CreateWordDocument(NotenizerWord word)
        {
            return new BsonDocument
            {
                { DBConstants.POSFieldName, word.POS },
                { DBConstants.IndexFieldName, word.Index }
            };
        }

        private static BsonDocument CreateDependencyDocument(NotenizerDependency dependency)
        {
            return CreateDependencyDocument(
                CreateWordDocument(dependency.Governor),
                CreateWordDocument(dependency.Dependent),
                dependency.Position);
        }

        public static BsonDocument CreateDependencyDocument(NotenizerDependency dependency, ComparisonType comparisonType, TokenType tokenType)
        {
            BsonDocument doc = CreateDependencyDocument(dependency);
            doc.Add(DBConstants.ComparisonTypeFieldName, new BsonInt32((int)comparisonType));
            doc.Add(DBConstants.TokenTypeFieldName, new BsonInt32((int)tokenType));

            return doc;
        }

        public static BsonDocument CreateDependencyDocument(BsonDocument governorDoc, BsonDocument dependentDoc, int position)
        {
            return new BsonDocument
                {
                    { DBConstants.GovernorFieldName, governorDoc },
                    { DBConstants.DependentFieldName, dependentDoc },
                    { DBConstants.PositionFieldName, new BsonInt32(position) }
                };
        }

        private static void AppendDependencyDocument(NotenizerDependency currentDependency, BsonDocument dependencyDoc, BsonArray destinationArr, Dictionary<String, BsonArray> dependencies)
        {
            if (dependencies.ContainsKey(currentDependency.Relation.ShortName))
                dependencies[currentDependency.Relation.ShortName].Add(dependencyDoc);
            else
            {
                BsonArray arr = new BsonArray { dependencyDoc };

                BsonDocument originalDependencyDoc = CreateNewOriginalDependencieDocumentEntry(dependencyDoc, currentDependency, arr);

                destinationArr.Add(originalDependencyDoc);
                dependencies.Add(currentDependency.Relation.ShortName, arr);
            }
        }

        private static BsonDocument CreateNewOriginalDependencieDocumentEntry(BsonDocument dependencyDoc, NotenizerDependency currentDependency, BsonArray arr)
        {
            return new BsonDocument
            {
                { DBConstants.DependencyNameFieldName, currentDependency.Relation.ShortName },
                { DBConstants.DependenciesFieldName, arr }
            };

        }

        public static FilterDefinition<BsonDocument> CreateFilterByDependencies(NotenizerSentence sentence)
        {
            return CreateFilterByDependencies(sentence.Dependencies, sentence.DistinctDependenciesCount);
        }

        public static FilterDefinition<BsonDocument> CreateFilterByDependencies(List<NotenizerDependency> dependencies, int size)
        {
            return Builders<BsonDocument>.Filter.Size(DBConstants.OriginalSentenceDependenciesFieldName, size) 
                    & Builders<BsonDocument>.Filter.All(DBConstants.OriginalSentenceDependenciesFieldName + "." + DBConstants.DependencyNameFieldName,
                        dependencies.Select(x => x.Relation.ShortName));
        }

        public static FilterDefinition<BsonDocument> CreateFilterById(String id)
        {
            ObjectId objectId = ObjectId.Parse(id);

            return Builders<BsonDocument>.Filter.Eq(DBConstants.IdFieldName, objectId);
        }
    }
}
