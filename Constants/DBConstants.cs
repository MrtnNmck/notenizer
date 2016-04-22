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

        public const String DependencyNameFieldName = "dependency_name";
        public const String NoteDependenciesFieldName = "noteDependencies";

        public const String NoteFieldName = "note";
        public const String CreatedByFieldName = "createdBy";
        public const String ArticleRefIdFieldName = "article_ref_id";
        public const String ComparisonTypeFieldName = "comparisonType";
      
        public const String TokenTypeFieldName = "tokenType";
        public const String AdditionalInformationFieldName = "additionalInformation";
        public const String CreatedAtFieldName = "createdAt";
        public const String UpdatedAtFieldName = "updatedAt";
        public const String IdFieldName = "_id";
        public const String AndSetPositionFieldName = "set_position";
        public const String RuleRefIdFieldName = "rule_ref_id";
        public const String AndRuleRefIdFieldName = "and_rule_ref_id";
        public const String BsonNullValue = "BsonNull";
        public const String SentenceTerminatorsFieldName = "sentence_terminators";
        public const String LemmaFieldName = "lemma";
        public const String NERFieldName = "ner";

        public const String TextFieldName = "text";
        public const String StructureRefIdFieldName = "structure_ref_id";
        public const String StructureDataFieldName = "structure_data";
        public const String SentenceRefIdFieldName = "sentence_ref_id";
        public const String NoteRefIdFieldName = "note_ref_id";

        public const String NotesCollectionName = "notes";
        public const String NoteRulesCollectionName = "rules";
        public const String AndParserRulesCollectionName = "and_rules";
        public const String ArticlesCollectionName = "articles";
        public const String StructuresCollectionName = "structures";
        public const String SentencesCollectionName = "sentences";
    }
}
