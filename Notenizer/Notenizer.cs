using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using java.util;
using java.io;
using edu.stanford.nlp.pipeline;
using Console = System.Console;
using edu.stanford.nlp.ling;
using System.Globalization;
using System.Threading;
using edu.stanford.nlp.trees;
using nsExtensions;
using nsNotenizerObjects;
using nsConstants;
using nsDB;
using nsEnums;
using nsParsers;
using nsComparsions;
using nsServices.DBServices;
using MongoDB.Bson;

namespace nsNotenizer
{
    public class Notenizer
    {
        #region Variables

        private bool _redirectOutputToFile = false;
        private String _redirectOutputToFileFileName = @"./out.txt";

        private StanfordCoreNLP _pipeline;
        private List<NotenizerNote> _notes;
        private StaticParser _staticParser;
        private ComparsionsManager _comparsionManager;

        #endregion Variables

        #region Constructors

        public Notenizer()
        {
            _staticParser = new StaticParser();
            _comparsionManager = new ComparsionsManager();
        }

        #endregion Constuctors

        #region Properties

        public List<NotenizerNote> Notes
        {
            get { return _notes; }
        }

        /// <summary>
        /// Singleton.
        /// </summary>
        private StanfordCoreNLP Pipeline
        {
            get
            {
                if (_pipeline == null)
                {
                    String jarRoot = @"stanford-corenlp-3.5.2-models";

                    // Annotation pipeline configuration
                    Properties properties = new Properties();
                    properties.setProperty("annotators", "tokenize, ssplit, pos, lemma, ner, parse");
                    properties.setProperty("sutime.binders", "0");
                    properties.setProperty("ner.useSUTime", "false");

                    // prevent time exception
                    CultureInfo cultureInfo = new CultureInfo("en-US");
                    Thread.CurrentThread.CurrentCulture = cultureInfo;
                    Thread.CurrentThread.CurrentUICulture = cultureInfo;

                    // We should change current directory, so StanfordCoreNLP could find all the model files automatically
                    String currentDirectory = Environment.CurrentDirectory;
                    Directory.SetCurrentDirectory(jarRoot);
                    _pipeline = new StanfordCoreNLP(properties);
                    Directory.SetCurrentDirectory(currentDirectory);
                }

                return _pipeline;
            }
        }

        #endregion Properties

        #region Methods

        public void RunCoreNLP(String text)
        {
            // needs to be before Annotation(text)
            // otherwise it throws error
            StanfordCoreNLP pipeLine = Pipeline;
            Annotation annotation = new Annotation(text);
            pipeLine.annotate(annotation);

            if (_redirectOutputToFile)
            {
                FileStream filestream = new FileStream(_redirectOutputToFileFileName, FileMode.OpenOrCreate, FileAccess.Write);
                var streamwriter = new StreamWriter(filestream);
                streamwriter.AutoFlush = true;
                Console.SetOut(streamwriter);
                Console.SetError(streamwriter);
            }

            // Result - Pretty Print
            using (ByteArrayOutputStream stream = new ByteArrayOutputStream())
            {
                pipeLine.prettyPrint(annotation, new PrintWriter(stream));
                Console.WriteLine(stream.toString());
                stream.close();
            }

            _notes = Parse(annotation);
            PrintNotes(_notes);
        }

        private NotenizerNoteRule GetRuleForSentence(NotenizerSentence sentence, out Note matchedNote)
        {
            Match match;
            Article article;
            Structure structure;
            Note matchedSentenceNote;
            NotenizerNoteRule matchedSentenceRule;
            NotenizerStructure matchedSentenceStructure;
            List<BsonDocument> sentencesWithSameStructure;

            matchedNote = null;
            structure = DocumentParser.GetHeighestMatch(
                sentence.Structure,
                DB.GetAll(DBConstants.StructuresCollectionName, DocumentCreator.CreateFilterByStructure(sentence)).Result,
                out match);

            if (structure == null)
                return null;

            matchedSentenceStructure = new NotenizerStructure(structure);

            sentencesWithSameStructure = DB.GetAll(
                DBConstants.SentencesCollectionName,
                DocumentCreator.CreateFilterById(DBConstants.StructureRefIdFieldName, matchedSentenceStructure.Structure.ID)).Result;

            nsNotenizerObjects.Sentence matchedSentence = null;

            if (sentencesWithSameStructure.Count > 0)
            {
                matchedSentence = DocumentParser.ParseSentence(sentencesWithSameStructure[0]);
                foreach (BsonDocument sentenceWithSameStructureLoop in sentencesWithSameStructure)
                {
                    if (sentenceWithSameStructureLoop[DBConstants.TextFieldName].AsString.Trim() == sentence.Sentence.Text)
                    {
                        match.Value = 100.0;
                        matchedSentence = DocumentParser.ParseSentence(sentenceWithSameStructureLoop);
                        break;
                    }
                }
            }

            if (matchedSentence == null)
                return null;

            article = DocumentParser.ParseArticle(
                DB.GetFirst(
                    DBConstants.ArticlesCollectionName,
                    DocumentCreator.CreateFilterById(sentence.Sentence.Article.ID)).Result);

            matchedSentenceNote = DocumentParser.ParseNote(
                DB.GetFirst(
                    DBConstants.NotesCollectionName,
                    DocumentCreator.CreateFilterById(matchedSentence.NoteID)).Result);

            matchedSentenceRule = DocumentParser.ParseRule(
                DB.GetFirst(
                    DBConstants.RulesCollectionName,
                    DocumentCreator.CreateFilterById(matchedSentence.RuleID)).Result);

            matchedSentenceRule.Structure = new NotenizerStructure(
                DocumentParser.ParseStructure(
                    DB.GetFirst(
                        DBConstants.StructuresCollectionName,
                        DocumentCreator.CreateFilterById(matchedSentenceRule.StructureID)).Result));

            matchedSentenceRule.Sentence = matchedSentence;
            matchedSentenceRule.Sentence.Article = article;
            matchedSentenceRule.Match = match;
            matchedNote = matchedSentenceNote;

            return matchedSentenceRule;
        }

        public NotenizerAndRule GetAndRuleForSentence(NotenizerRule rule, String andRuleId)
        {
            NotenizerAndRule andRule;

            andRule = DocumentParser.ParseAndRule(
                DB.GetFirst(
                    DBConstants.AndRulesCollectionName,
                    DocumentCreator.CreateFilterById(andRuleId)).Result);

            andRule.Structure = new NotenizerStructure(
                DocumentParser.ParseStructure(
                    DB.GetFirst(
                        DBConstants.StructuresCollectionName,
                        DocumentCreator.CreateFilterById(andRule.StructureID)).Result));

            andRule.Sentence = rule.Sentence;

            return andRule;
        }

        private Article GetArticle(String text)
        {
            Article article;
            List<BsonDocument> articles = DB.GetAll(DBConstants.ArticlesCollectionName, DocumentCreator.CreateFilter(DBConstants.TextFieldName, text)).Result;

            if (articles.Count == 0)
            {
                article = new Article(String.Empty, DateTime.Now, DateTime.Now, text);
                article.ID = DB.InsertToCollection(DBConstants.ArticlesCollectionName, DocumentCreator.CreateArticleDocument(article)).Result;
            }
            else
                article = DocumentParser.ParseArticle(articles[0]);

            return article;
        }

        /// <summary>
        /// Parses the sentence.
        /// </summary>
        /// <param name="annotation"></param>
        /// <returns></returns>
        public List<NotenizerNote> Parse(Annotation annotation)
        {
            List<NotenizerNote> sentencesNoted = new List<NotenizerNote>();
            List<NotenizerNote> notesToSave = new List<NotenizerNote>();

            Article article = GetArticle(annotation.ToString().Trim());

            // ================== REFACTORED PART HERE ======================
            foreach (Annotation sentenceLoop in annotation.get(typeof(CoreAnnotations.SentencesAnnotation)) as ArrayList)
            {
                NotenizerSentence sentence = new NotenizerSentence(sentenceLoop, article);
                Note matchedNote;

                NotenizerNoteRule rule = GetRuleForSentence(sentence, out matchedNote);

                if (rule != null && rule.Structure.Dependencies != null && rule.Structure.Dependencies.Count > 0)
                {
                    NotenizerNote parsedNote = ApplyRule(sentence, rule);
                    parsedNote.Note = matchedNote;

                    if (parsedNote.Note.AndRuleID != DBConstants.BsonNullValue)
                        parsedNote.AndRule = GetAndRuleForSentence(rule, parsedNote.Note.AndRuleID);

                    Console.WriteLine("Parsed note: " + parsedNote.OriginalSentence + " ===> " + parsedNote.Text);
                    sentencesNoted.Add(parsedNote);

                    continue;
                }

                NotenizerNote note = _staticParser.Parse(sentence);
                notesToSave.Add(note);
            }

            // inserting into DB AFTER ALL sentences from article were processed
            // to avoid processed sentence to affect processing other sentences from article
            foreach (NotenizerNote sentenceNotedLoop in notesToSave)
            {
                // save rule's structure
                NotenizerNoteRule rule = sentenceNotedLoop.CreateRule();
                sentenceNotedLoop.CreateStructure();
                rule.Structure.Structure.ID = DB.InsertToCollection(DBConstants.StructuresCollectionName, DocumentCreator.CreateStructureDocument(rule)).Result;

                // save sentence's structure
                NotenizerStructure sentenceStructure = sentenceNotedLoop.OriginalSentence.Structure;
                sentenceStructure.Structure.ID = DB.InsertToCollection(DBConstants.StructuresCollectionName, DocumentCreator.CreateStructureDocument(sentenceStructure)).Result;

                // save rule
                rule.ID = DB.InsertToCollection(DBConstants.RulesCollectionName, DocumentCreator.CreateRuleDocument(rule)).Result;

                // save note
                Note note = sentenceNotedLoop.CreateNote();
                note.ID = DB.InsertToCollection(DBConstants.NotesCollectionName, DocumentCreator.CreateNoteDocument(
                    sentenceNotedLoop,
                    rule.ID,
                    String.Empty)).Result;

                // save sentence
                sentenceNotedLoop.OriginalSentence.Sentence.ID = DB.InsertToCollection(DBConstants.SentencesCollectionName, DocumentCreator.CreateSentenceDocument(
                    sentenceNotedLoop.OriginalSentence,
                    sentenceStructure.Structure.ID,
                    article.ID,
                    rule.ID,
                    String.Empty,
                    note.ID)).Result;

                Console.WriteLine("Parsed note: " + sentenceNotedLoop.OriginalSentence + " ===> " + sentenceNotedLoop.Text);
                sentencesNoted.Add(sentenceNotedLoop);
            }

            return sentencesNoted;
        }

        /// <summary>
        /// Prints notes to console.
        /// </summary>
        /// <param name="notes"></param>
        public void PrintNotes(List<NotenizerNote> notes)
        {
            Console.WriteLine();
            Console.WriteLine("<======== NOTES ========>");

            foreach (NotenizerNote noteLoop in notes)
            {
                Console.WriteLine(noteLoop.OriginalSentence + " ---> " + noteLoop.Text);
            }
        }

        /// <summary>
        /// Applies rule.
        /// Parses sentence by applied rule.
        /// </summary>
        /// <param name="sentence">Sentence to apply rule to</param>
        /// <param name="rule">Rule for parsing to apply</param>
        /// <returns></returns>
        public NotenizerNote ApplyRule(NotenizerSentence sentence, NotenizerRule rule)
        {
            NotenizerNote note = new NotenizerNote(sentence);
            NotePart notePart = new NotePart(sentence);

            foreach (NotenizerDependency ruleLoop in rule.Structure.Dependencies)
            {
                ApplyRule(sentence, ruleLoop, notePart);
            }

            note.Add(notePart);

            if (rule is NotenizerNoteRule)
                ApplyRule(note, rule as NotenizerNoteRule);
            else if (rule is NotenizerAndRule)
                ApplyRule(note, rule as NotenizerAndRule);
            //note.Note = rule.Note;
            note.Structure = rule.Structure;
            return note;
        }

        private void ApplyRule(NotenizerNote note, NotenizerNoteRule noteRule)
        {
            note.SplitToSentences(noteRule.SentencesTerminators);
            note.Rule = noteRule;
        }

        private void ApplyRule(NotenizerNote note, NotenizerAndRule andParserRule)
        {
            note.SplitToSentences(andParserRule.SentenceTerminator);
            note.AndRule = andParserRule;
        }

        /// <summary>
        /// Applies rule.
        /// Parses sentence by applied rule and part of note of original sentence.
        /// </summary>
        /// <param name="sentence">Sentence to apply rule to</param>
        /// <param name="rule">Rule for parsing to apply</param>
        /// <param name="notePart">Part of note</param>
        private void ApplyRule(NotenizerSentence sentence, NotenizerDependency rule, NotePart notePart)
        {
            NotenizerDependency dependency = null;// = sentence.FindDependency(rule);
            double match = 0.0;
            double currentMatch = 0.0;

            foreach (NotenizerDependency dependencyLoop in sentence.Structure.FindDependencies(rule))
            {
                if (dependencyLoop == null)
                    continue;

                if ((currentMatch = _comparsionManager.Compare(rule, dependencyLoop, sentence.Structure.Dependencies.Count)) > match)
                {
                    match = currentMatch;
                    dependency = dependencyLoop;
                }
            }

            if (dependency != null)
            {
                NoteParticle dependencyObj = new NoteParticle(dependency, rule.TokenType, rule.Position);
                notePart.Add(dependencyObj);
            }
        }

        #endregion Methods
    }
}
