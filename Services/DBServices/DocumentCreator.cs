using MongoDB.Bson;
using MongoDB.Driver;
using nsConstants;
using nsEnums;
using nsExtensions;
using nsInterfaces;
using nsNotenizerObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsServices.DBServices
{
    public static class DocumentCreator
    {
        #region Properties

        #endregion Properties

        #region Methods

        public static BsonDocument CreateNoteDocument(NotenizerNote note, String noteRuleId, String andParseRuleId)
        {
            BsonDocument doc = new BsonDocument();
            BsonValue andParserRuleObjectId;
            BsonValue ruleObjectId;
            BsonArray originalDepencenciesArr = new BsonArray();
            Dictionary<String, BsonArray> dependencies = new Dictionary<String, BsonArray>();

            note.Note.RuleID = noteRuleId;
            note.Note.AndRuleID = andParseRuleId;

            ruleObjectId = noteRuleId.ToObjectId();
            andParserRuleObjectId = andParseRuleId.ToObjectId();

            doc.Add(DBConstants.TextFieldName, new BsonString(note.Text));
            doc.Add(DBConstants.RuleRefIdFieldName, ruleObjectId);
            doc.Add(DBConstants.AndRuleRefIdFieldName, andParserRuleObjectId);
            doc = doc.InsertNotenizerSpecificFields(note.Note);

            return doc;
        }

        public static BsonDocument CreateSentenceDocument(NotenizerSentence sentence, String structureId, String articleId, String ruleId, String andRuleId, String noteId)
        {
            BsonDocument doc = new BsonDocument();
            BsonValue articleObjectId;
            BsonValue andParserRuleObjectId;
            BsonValue ruleObjectId;
            BsonValue structureObjectId;
            BsonValue noteObjectId;
            BsonArray originalDepencenciesArr = new BsonArray();
            Dictionary<String, BsonArray> dependencies = new Dictionary<String, BsonArray>();

            sentence.Sentence.StructureID = structureId;
            sentence.Sentence.ArticleID = articleId;
            sentence.Sentence.RuleID = ruleId;
            sentence.Sentence.AndRuleID = andRuleId;
            sentence.Sentence.NoteID = noteId;

            articleObjectId = articleId.ToObjectId();
            ruleObjectId = ruleId.ToObjectId();
            andParserRuleObjectId = andRuleId.ToObjectId();
            structureObjectId = structureId.ToObjectId();
            noteObjectId = noteId.ToObjectId();

            doc.Add(DBConstants.TextFieldName, new BsonString(sentence.ToString()));
            doc.Add(DBConstants.ArticleRefIdFieldName, articleObjectId);
            doc.Add(DBConstants.RuleRefIdFieldName, ruleObjectId);
            doc.Add(DBConstants.AndRuleRefIdFieldName, andParserRuleObjectId);
            doc.Add(DBConstants.StructureRefIdFieldName, structureObjectId);
            doc.Add(DBConstants.NoteRefIdFieldName, noteObjectId);
            doc = doc.InsertNotenizerSpecificFields(sentence.Sentence);
            
            return doc;
        }

        public static BsonDocument CreateStructureDocument(NotenizerSentence sentence)
        {
            return CreateStructureDocument(sentence.Structure);
        }

        public static BsonDocument CreateStructureDocument(NotenizerNoteRule rule)
        {
            return CreateStructureDocument(rule.Structure, true);
        }

        public static BsonDocument CreateStructureDocument(NotenizerStructure structure, bool additionalInfo = false)
        {
            BsonDocument document = new BsonDocument();
            BsonArray structureDataArray = new BsonArray();
            BsonDocument dependencyDoc;
            Dictionary<String, BsonArray> dependenciesDictionary = new Dictionary<String, BsonArray>();

            foreach (NotenizerDependency dependencyLoop in structure.Dependencies)
            {
                if (additionalInfo)
                    dependencyDoc = CreateDependencyDocument(dependencyLoop, dependencyLoop.ComparisonType, dependencyLoop.TokenType);
                else
                    dependencyDoc = CreateDependencyDocument(dependencyLoop);

                AppendDependencyDocument(dependencyLoop, dependencyDoc, structureDataArray, dependenciesDictionary);
            }

            document.Add(DBConstants.StructureDataFieldName, structureDataArray);
            document = document.InsertNotenizerSpecificFields(structure.Structure);

            return document;
        }

        public static BsonDocument CreateRuleDocument(NotenizerNoteRule rule)
        {
            BsonDocument document = new BsonDocument();
            BsonValue structureObjectId;

            structureObjectId = rule.Structure.Structure.ID.ToObjectId();

            document.Add(DBConstants.StructureRefIdFieldName, structureObjectId);
            document.Add(DBConstants.SentenceTerminatorsFieldName, new BsonArray(rule.SentencesTerminators));
            document = document.InsertNotenizerSpecificFields(rule);

            return document;
        }

        public static BsonDocument CreateRuleDocument(NotenizerAndRule rule)
        {
            BsonDocument document = new BsonDocument();
            BsonValue structureObjectId;

            structureObjectId = rule.Structure.Structure.ID.ToObjectId();

            document.Add(DBConstants.StructureRefIdFieldName, structureObjectId);
            document.Add(DBConstants.SentenceTerminatorsFieldName, new BsonInt32(rule.SentenceTerminator));
            document.Add(DBConstants.AndSetPositionFieldName, new BsonInt32(rule.SetsPosition));
            document = document.InsertNotenizerSpecificFields(rule);

            return document;
        }

        public static BsonDocument CreateArticleDocument(Article article)
        {
            BsonDocument doc = new BsonDocument();

            doc.Add(DBConstants.TextFieldName, new BsonString(article.Value));
            doc = doc.InsertNotenizerSpecificFields(article);

            return doc;
        }

        private static BsonDocument CreateWordDocument(NotenizerWord word)
        {
            return new BsonDocument
            {
                { DBConstants.POSFieldName, word.POS.Tag },
                { DBConstants.IndexFieldName, word.Index },
                { DBConstants.NERFieldName, word.NamedEntity.Value },
                { DBConstants.LemmaFieldName, word.Lemma }
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
                { DBConstants.RelationNameFieldName, currentDependency.Relation.ShortName },
                { DBConstants.DependenciesFieldName, arr }
            };

        }

        public static FilterDefinition<BsonDocument> CreateFilterByStructure(NotenizerSentence sentence)
        {
            return CreateFilterByStructure(sentence.Structure);
        }

        public static FilterDefinition<BsonDocument> CreateFilterByStructure(NotenizerStructure structure)
        {
            return CreateFilterByStructure(structure.Dependencies, structure.DistinctDependenciesCount);
        }

        public static FilterDefinition<BsonDocument> CreateFilterByStructure(NotenizerDependencies dependencies, int size)
        {
            return Builders<BsonDocument>.Filter.Size(DBConstants.StructureDataFieldName, size)
                    & Builders<BsonDocument>.Filter.All(DBConstants.StructureDataFieldName + "." + DBConstants.RelationNameFieldName,
                        dependencies.Select(x => x.Relation.ShortName));
        }

        public static FilterDefinition<BsonDocument> CreateFilter(String fieldName, String fieldValue)
        {
            return Builders<BsonDocument>.Filter.Eq(fieldName, fieldValue);
        }

        public static FilterDefinition<BsonDocument> CreateFilter(String fieldName, ObjectId fieldValue)
        {
            return Builders<BsonDocument>.Filter.Eq(fieldName, fieldValue);
        }

        public static FilterDefinition<BsonDocument> CreateFilterById(String id)
        {
            return CreateFilterById(DBConstants.IdFieldName, id);
        }

        public static FilterDefinition<BsonDocument> CreateFilterById(String fieldName, String id)
        {
            ObjectId objectId = ObjectId.Parse(id);

            return CreateFilter(fieldName, objectId);
        }

        private static BsonDocument InsertNotenizerSpecificFields(this BsonDocument doc, IPersistable persistableObj)
        {
            return doc.InsertNotenizerSpecificFields(persistableObj.CreatedAt, persistableObj.UpdatedAt);
        }

        private static BsonDocument InsertNotenizerSpecificFields(this BsonDocument doc, DateTime createdAt, DateTime updatedAt)
        {
            doc.Add(DBConstants.CreatedAtFieldName, new BsonDateTime(createdAt));
            doc.Add(DBConstants.UpdatedAtFieldName, new BsonDateTime(updatedAt));

            return doc;
        }

        #endregion Methods
    }
}
