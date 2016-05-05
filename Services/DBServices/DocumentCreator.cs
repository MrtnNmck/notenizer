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
    /// <summary>
    /// handles creations of database's documents.
    /// </summary>
    public static class DocumentCreator
    {
        #region Methods

        /// <summary>
        /// Creates document for note.
        /// </summary>
        /// <param name="note">Note</param>
        /// <param name="noteRuleId">ID of rule</param>
        /// <param name="andParseRuleId">ID of and-rule</param>
        /// <returns></returns>
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

        /// <summary>
        /// Creates document for sentence.
        /// </summary>
        /// <param name="sentence">Sentence</param>
        /// <param name="structureId">ID of structure</param>
        /// <param name="articleId">ID of article</param>
        /// <param name="ruleId">ID of rule</param>
        /// <param name="andRuleId">ID of and-rule</param>
        /// <param name="noteId">ID of note</param>
        /// <returns></returns>
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

        /// <summary>
        /// Creates document for structure of sentence.
        /// </summary>
        /// <param name="sentence">Sentence</param>
        /// <returns></returns>
        public static BsonDocument CreateStructureDocument(NotenizerSentence sentence)
        {
            return CreateStructureDocument(sentence.Structure);
        }

        /// <summary>
        /// Creates document for structure of rule.
        /// </summary>
        /// <param name="rule">Rule</param>
        /// <returns></returns>
        public static BsonDocument CreateStructureDocument(NotenizerNoteRule rule)
        {
            return CreateStructureDocument(rule.Structure, true);
        }

        /// <summary>
        /// Creates document for structure.
        /// </summary>
        /// <param name="structure">Structure</param>
        /// <param name="additionalInfo">Flag if add aditional information</param>
        /// <returns></returns>
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

        /// <summary>
        /// Creates document for rule.
        /// </summary>
        /// <param name="rule"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Creates document for and-rule.
        /// </summary>
        /// <param name="rule"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Creates document for article.
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        public static BsonDocument CreateArticleDocument(Article article)
        {
            BsonDocument doc = new BsonDocument();

            doc.Add(DBConstants.TextFieldName, new BsonString(article.Text));
            doc = doc.InsertNotenizerSpecificFields(article);

            return doc;
        }

        /// <summary>
        /// Creates document for word.
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Creates document for depenendcy.
        /// </summary>
        /// <param name="dependency"></param>
        /// <returns></returns>
        private static BsonDocument CreateDependencyDocument(NotenizerDependency dependency)
        {
            return CreateDependencyDocument(
                CreateWordDocument(dependency.Governor),
                CreateWordDocument(dependency.Dependent),
                dependency.Position);
        }

        /// <summary>
        /// Creates document for dependency.
        /// </summary>
        /// <param name="dependency">Dependency</param>
        /// <param name="comparisonType">Comparison type</param>
        /// <param name="tokenType">Token type</param>
        /// <returns></returns>
        public static BsonDocument CreateDependencyDocument(NotenizerDependency dependency, ComparisonType comparisonType, TokenType tokenType)
        {
            BsonDocument doc = CreateDependencyDocument(dependency);
            doc.Add(DBConstants.ComparisonTypeFieldName, new BsonInt32((int)comparisonType));
            doc.Add(DBConstants.TokenTypeFieldName, new BsonInt32((int)tokenType));

            return doc;
        }

        /// <summary>
        /// Creates document for dependency.
        /// </summary>
        /// <param name="governorDoc">Governor token document</param>
        /// <param name="dependentDoc">Depenendet token document</param>
        /// <param name="position">Position of dependency</param>
        /// <returns></returns>
        public static BsonDocument CreateDependencyDocument(BsonDocument governorDoc, BsonDocument dependentDoc, int position)
        {
            return new BsonDocument
                {
                    { DBConstants.GovernorFieldName, governorDoc },
                    { DBConstants.DependentFieldName, dependentDoc },
                    { DBConstants.PositionFieldName, new BsonInt32(position) }
                };
        }

        /// <summary>
        /// Appends depenecies document.
        /// </summary>
        /// <param name="currentDependency"></param>
        /// <param name="dependencyDoc"></param>
        /// <param name="destinationArr"></param>
        /// <param name="dependencies"></param>
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

        /// <summary>
        /// Creates document for dependencies.
        /// </summary>
        /// <param name="dependencyDoc"></param>
        /// <param name="currentDependency"></param>
        /// <param name="arr"></param>
        /// <returns></returns>
        private static BsonDocument CreateNewOriginalDependencieDocumentEntry(BsonDocument dependencyDoc, NotenizerDependency currentDependency, BsonArray arr)
        {
            return new BsonDocument
            {
                { DBConstants.RelationNameFieldName, currentDependency.Relation.ShortName },
                { DBConstants.DependenciesFieldName, arr }
            };

        }
        
        /// <summary>
        /// Creates filter by sentence's structure.
        /// </summary>
        /// <param name="sentence"></param>
        /// <returns></returns>
        public static FilterDefinition<BsonDocument> CreateFilterByStructure(NotenizerSentence sentence)
        {
            return CreateFilterByStructure(sentence.Structure);
        }

        /// <summary>
        /// Create filter by srtucture.
        /// </summary>
        /// <param name="structure"></param>
        /// <returns></returns>
        public static FilterDefinition<BsonDocument> CreateFilterByStructure(NotenizerStructure structure)
        {
            return CreateFilterByStructure(structure.Dependencies, structure.DistinctDependenciesCount);
        }

        /// <summary>
        /// Creates filter by strucuture/
        /// </summary>
        /// <param name="dependencies">Structure's dependencies</param>
        /// <param name="size">Size</param>
        /// <returns></returns>
        public static FilterDefinition<BsonDocument> CreateFilterByStructure(NotenizerDependencies dependencies, int size)
        {
            return Builders<BsonDocument>.Filter.Size(DBConstants.StructureDataFieldName, size)
                    & Builders<BsonDocument>.Filter.All(DBConstants.StructureDataFieldName + "." + DBConstants.RelationNameFieldName,
                        dependencies.Select(x => x.Relation.ShortName));
        }

        /// <summary>
        /// Creates filter for field.
        /// </summary>
        /// <param name="fieldName">Name of field</param>
        /// <param name="fieldValue">Value of field</param>
        /// <returns></returns>
        public static FilterDefinition<BsonDocument> CreateFilter(String fieldName, String fieldValue)
        {
            return Builders<BsonDocument>.Filter.Eq(fieldName, fieldValue);
        }

        /// <summary>
        /// Creates filter for ObjectId field.
        /// </summary>
        /// <param name="fieldName">Name of field</param>
        /// <param name="fieldValue">Value of field</param>
        /// <returns></returns>
        public static FilterDefinition<BsonDocument> CreateFilter(String fieldName, ObjectId fieldValue)
        {
            return Builders<BsonDocument>.Filter.Eq(fieldName, fieldValue);
        }

        /// <summary>
        /// Creates filter by ID.
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public static FilterDefinition<BsonDocument> CreateFilterById(String id)
        {
            return CreateFilterById(DBConstants.IdFieldName, id);
        }

        /// <summary>
        /// Creates filter by ID.
        /// </summary>
        /// <param name="fieldName">Name of field</param>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public static FilterDefinition<BsonDocument> CreateFilterById(String fieldName, String id)
        {
            ObjectId objectId = ObjectId.Parse(id);

            return CreateFilter(fieldName, objectId);
        }

        /// <summary>
        /// Inserts notenizer specific fields into document.
        /// </summary>
        /// <param name="doc">document</param>
        /// <param name="persistableObj">Persistable object</param>
        /// <returns></returns>
        private static BsonDocument InsertNotenizerSpecificFields(this BsonDocument doc, IPersistable persistableObj)
        {
            return doc.InsertNotenizerSpecificFields(persistableObj.CreatedAt, persistableObj.UpdatedAt);
        }

        /// <summary>
        /// Inserts notenizer specific fields into document.
        /// </summary>
        /// <param name="doc">Document</param>
        /// <param name="createdAt">Created at timestamp</param>
        /// <param name="updatedAt">Updated at timestamp</param>
        /// <returns></returns>
        private static BsonDocument InsertNotenizerSpecificFields(this BsonDocument doc, DateTime createdAt, DateTime updatedAt)
        {
            doc.Add(DBConstants.CreatedAtFieldName, new BsonDateTime(createdAt));
            doc.Add(DBConstants.UpdatedAtFieldName, new BsonDateTime(updatedAt));

            return doc;
        }

        #endregion Methods
    }
}
