using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsConstants
{
    public class DBConstants
    {
        public const String DatabaseName = "notenizer";
        public const String POSFieldName = "pos";
        public const String IndexFieldName = "index";
        public const String GovernorFieldName = "governor";
        public const String DependentFieldName = "dependent";
        public const String PositionFieldName = "position";
        public const String DependenciesFieldName = "dependencies";
        public const String OriginalSentenceDependenciesFieldName = "originalDependencies";
        public const String OriginalSentenceFieldName = "originalSentence";
        public const String DependencyNameFieldName = "dependencyName";
        public const String NoteDependenciesFieldName = "noteDependencies";
        public const String NoteFieldName = "note";
        public const String CreatedByFieldName = "createdBy";
        public const String ArticleIdFieldName = "articleRefId";
        public const String ComparisonTypeFieldName = "comparisonType";
        public const String SentencesEndsFieldName = "sentencesEnds";
        public const String TokenTypeFieldName = "tokenType";
        public const String AdditionalInformationFieldName = "additionalInformation";
        public const String CreatedAtFieldName = "createdAt";
        public const String UpdatedAtFieldName = "updatedAt";
        public const String IdFieldName = "_id";
        public const String AndSetsPositionsFieldName = "setsPosition";
        public const String NoteRuleRefIdFieldName = "noteRuleRefId";
        public const String AndParserRuleRefIdFieldName = "andParserRuleRefId";
        public const String BsonNullValue = "BsonNull";
        public const String SentenceEndFieldname = "sentenceEnd";
        public const String LemmaFieldName = "lemma";
        public const String NERFieldName = "ner";
        public const String ArticleFieldName = "article";

        public const String NotesCollectionName = "notes";
        public const String NoteRulesCollectionName = "noteRules";
        public const String AndParserRulesCollectionName = "andParserRules";
        public const String ArticlesCollectionName = "articles";
    }
}
