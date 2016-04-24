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
        private bool _redirectOutputToFile = false;
        private String _redirectOutputToFileFileName = @"./out.txt";
        List<NotenizerNote> _notes;
        StanfordCoreNLP _pipeline;
        private ComparsionsManager _comparsionManager;

		public Notenizer()
		{
            _comparsionManager = new ComparsionsManager();
		}

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

        public void InitCoreNLP()
        {
            
        }

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

        private NotenizerNoteRule GetRuleForSentence(NotenizerSentence sentence)
        {
            // vyhladat vyhovujuce struktury
            // zistit najvacsiu zhodu so strukturou
            // pre tu strukturu najst vetu, pravidlo a and-pravidlo
            // pre pravidlo najst poznamku
            Match match;
            Structure structure;
            NotenizerStructure matchedSentenceStructure;
            List<MongoDB.Bson.BsonDocument> sentencesWithSameStructure;
            Article article;
            Note matchedSentenceNote;
            NotenizerNoteRule matchedSentenceRule;

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
                    DocumentCreator.CreateFilterById(matchedSentence.ArticleID)).Result);

            matchedSentenceNote = DocumentParser.ParseNote(
                DB.GetFirst(
                    DBConstants.NotesCollectionName,
                    DocumentCreator.CreateFilterById(matchedSentence.NoteID)).Result);

            matchedSentenceRule = DocumentParser.ParseRule(
                DB.GetFirst(
                    DBConstants.NoteRulesCollectionName,
                    DocumentCreator.CreateFilterById(matchedSentence.RuleID)).Result);

            matchedSentenceRule.Structure = new NotenizerStructure(
                DocumentParser.ParseStructure(
                    DB.GetFirst(
                        DBConstants.StructuresCollectionName,
                        DocumentCreator.CreateFilterById(matchedSentenceRule.StructureID)).Result));

            matchedSentenceRule.Sentence = matchedSentence;
            matchedSentenceRule.Sentence.Article = article;
            matchedSentenceRule.Note = matchedSentenceNote;
            matchedSentenceRule.Match = match;

            return matchedSentenceRule;
        }

        public NotenizerAndRule GetAndRuleForSentence(NotenizerRule rule)
        {
            NotenizerAndRule andRule;

            andRule = DocumentParser.ParseAndRule(
                DB.GetFirst(
                    DBConstants.AndParserRulesCollectionName,
                    DocumentCreator.CreateFilterById(rule.Sentence.AndRuleID)).Result);

            andRule.Structure = new NotenizerStructure(
                DocumentParser.ParseStructure(
                    DB.GetFirst(
                        DBConstants.StructuresCollectionName,
                        DocumentCreator.CreateFilterById(andRule.StructureID)).Result));

            andRule.Sentence = rule.Sentence;
            andRule.Note = rule.Note;

            return andRule;
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

            // ================== REFACTORED PART HERE ======================
            foreach (Annotation sentenceLoop in annotation.get(typeof(CoreAnnotations.SentencesAnnotation)) as ArrayList)
            {
                NotenizerSentence sentence = new NotenizerSentence(sentenceLoop);

                NotenizerNoteRule rule = GetRuleForSentence(sentence);

                if (rule != null && rule.Structure.Dependencies != null && rule.Structure.Dependencies.Count > 0)
                {
                    NotenizerNote parsedNote = ApplyRule(sentence, rule);

                    if (rule.Note.AndRuleID != DBConstants.BsonNullValue)
                        parsedNote.AndParserRule = GetAndRuleForSentence(rule);

                    Console.WriteLine("Parsed note: " + parsedNote.OriginalSentence + " ===> " + parsedNote.Text);
                    sentencesNoted.Add(parsedNote);

                    continue;
                }

                NotenizerNote note = StaticParser(sentence);
                notesToSave.Add(note);
            }

            Article article;
            List<MongoDB.Bson.BsonDocument> articles = DB.GetAll(DBConstants.ArticlesCollectionName, DocumentCreator.CreateFilter(DBConstants.TextFieldName, annotation.ToString().Trim())).Result;

            if (articles.Count == 0)
            {
                article = new Article(String.Empty, DateTime.Now, DateTime.Now, annotation.ToString().Trim());
                article.ID = DB.InsertToCollection(DBConstants.ArticlesCollectionName, DocumentCreator.CreateArticleDocument(article)).Result;
            }
            else
                article = DocumentParser.ParseArticle(articles[0]);

            // inserting into DB AFTER ALL sentences from article were processed
            // to avoid processed sentence to affect processing other sentences from article
            foreach (NotenizerNote sentenceNotedLoop in notesToSave)
            {
                // ulozit struktury: pravidla, vety
                // ulozit pravidlo
                // ulozit vetu
                // ulozit poznamku

                // save rule's structure
                NotenizerNoteRule rule = sentenceNotedLoop.CreateRule();
                sentenceNotedLoop.CreateStructure();
                rule.Structure.Structure.ID = DB.InsertToCollection(DBConstants.StructuresCollectionName, DocumentCreator.CreateStructureDocument(rule)).Result;

                // save sentence's structure
                NotenizerStructure sentenceStructure = sentenceNotedLoop.OriginalSentence.Structure;
                sentenceStructure.Structure.ID = DB.InsertToCollection(DBConstants.StructuresCollectionName, DocumentCreator.CreateStructureDocument(sentenceStructure)).Result;

                // save rule
                rule.ID = DB.InsertToCollection(DBConstants.NoteRulesCollectionName, DocumentCreator.CreateRuleDocument(rule)).Result;


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

        private NotenizerNote StaticParser(NotenizerSentence sentence)
        {
            NotenizerNote note = new NotenizerNote(sentence);

            foreach (NotenizerDependency dependencyLoop in sentence.Structure.Dependencies)
            {
                if (dependencyLoop.Relation.IsNominalSubject()
                    && !((note.Structure.CompressedDependencies.ContainsKey(GrammaticalConstants.NominalSubject) 
                            && note.Structure.CompressedDependencies[GrammaticalConstants.NominalSubject].Any(x => x.Key == dependencyLoop.Key))
                        || (note.Structure.CompressedDependencies.ContainsKey(GrammaticalConstants.NominalSubjectPassive) 
                            && note.Structure.CompressedDependencies[GrammaticalConstants.NominalSubjectPassive].Any(x => x.Key == dependencyLoop.Key))))
                {
                    NotePart notePart = new NotePart(sentence);

                    NoteParticle nsubj = new NoteParticle(dependencyLoop, TokenType.Dependent);
                    notePart.Add(nsubj);

                    String pos = dependencyLoop.Governor.POS.Tag;
                    if (POSConstants.NounLikePOS.Contains(pos))
                    {
                        NotenizerDependency compound = sentence.Structure.GetDependencyByShortName(
                            dependencyLoop,
                            ComparisonType.DependentToGovernor,
                            GrammaticalConstants.CompoudModifier);

                        if (compound != null)
                        {
                            NoteParticle compoundObj = new NoteParticle(compound, TokenType.Dependent);
                            notePart.Add(compoundObj);
                        }

                        NotenizerDependency aux = sentence.Structure.GetDependencyByShortName(
                            dependencyLoop,
                            ComparisonType.GovernorToGovernor,
                            GrammaticalConstants.AuxModifier,
                            GrammaticalConstants.AuxModifierPassive);

                        if (aux != null)
                        {
                            NoteParticle auxObj = new NoteParticle(aux, TokenType.Dependent);
                            notePart.Add(auxObj);
                        }

                        NotenizerDependency cop = sentence.Structure.GetDependencyByShortName(dependencyLoop, ComparisonType.GovernorToGovernor, GrammaticalConstants.Copula);

                        if (cop != null)
                        {
                            NoteParticle copObj = new NoteParticle(cop, TokenType.Dependent);
                            notePart.Add(copObj);
                        }

                        List<NotenizerDependency> conjuctions = sentence.Structure.GetDependenciesByShortName(
                            dependencyLoop,
                            ComparisonType.GovernorToGovernor,
                            GrammaticalConstants.Conjuction);


                        String specific = String.Empty;
                        if (conjuctions != null && conjuctions.Count > 0)
                        {
                            List<NotenizerDependency> filteredConjs = FilterByPOS(conjuctions, POSConstants.ConjustionPOS);

                            foreach (NotenizerDependency filteredConjLoop in filteredConjs)
                            {
                                NotenizerDependency cc = sentence.Structure.GetDependencyByShortName(dependencyLoop, ComparisonType.GovernorToGovernor, GrammaticalConstants.CoordinatingConjuction);

                                if (cc.Dependent.Word == filteredConjLoop.Relation.Specific
                                    && sentence.Structure.DependencyIndex(filteredConjLoop) > sentence.Structure.DependencyIndex(cc))
                                {
                                    NoteParticle ccObj = new NoteParticle(cc, TokenType.Dependent);
                                    NoteParticle filteredConjObj = new NoteParticle(filteredConjLoop, TokenType.Dependent);

                                    notePart.Add(ccObj);
                                    notePart.Add(filteredConjObj);
                                }
                            }
                        }

                        // <== NMODS ==>
                        List<NotenizerDependency> nmodsList = sentence.Structure.GetDependenciesByShortName(
                            dependencyLoop, ComparisonType.GovernorToGovernor, GrammaticalConstants.NominalModifier);

                        if (nmodsList != null && nmodsList.Count > 0)
                        {
                            NotenizerDependency first = nmodsList.First();
                            NotenizerDependency neg = sentence.Structure.GetDependencyByShortName(
                                first, ComparisonType.DependentToGovernor, GrammaticalConstants.NegationModifier);

                            if (neg == null)
                            {
                                NoteParticle firstObj = new NoteParticle(first.Relation.AdjustedSpecific + NotenizerConstants.WordDelimeter + first.Dependent.Word, first, TokenType.Dependent);
                                notePart.Add(firstObj);
                            }
                            else
                            {
                                NoteParticle negObj = new NoteParticle(neg, TokenType.Dependent);
                                NoteParticle firstObj = new NoteParticle(first.Relation.AdjustedSpecific + NotenizerConstants.WordDelimeter + first.Dependent.Word, first, TokenType.Dependent);

                                notePart.Add(negObj);
                                notePart.Add(firstObj);
                            }

                            // second nmod depending on first one
                            NotenizerDependency nmodSecond = sentence.Structure.GetDependencyByShortName(
                                first, ComparisonType.DependentToGovernor, GrammaticalConstants.NominalModifier);

                            if (nmodSecond != null)
                            {
                                neg = sentence.Structure.GetDependencyByShortName(
                                first, ComparisonType.GovernorToGovernor, GrammaticalConstants.NegationModifier);

                                if (neg == null)
                                {
                                    NoteParticle secondObj = new NoteParticle(nmodSecond.Relation.AdjustedSpecific + NotenizerConstants.WordDelimeter + nmodSecond.Dependent.Word, nmodSecond, TokenType.Dependent);
                                    notePart.Add(secondObj);
                                }
                                else
                                {
                                    NoteParticle negObj = new NoteParticle(neg, TokenType.Dependent);
                                    NoteParticle secondObj = new NoteParticle(nmodSecond.Relation.AdjustedSpecific + NotenizerConstants.WordDelimeter + nmodSecond.Dependent.Word, nmodSecond, TokenType.Dependent);

                                    notePart.Add(negObj);
                                    notePart.Add(secondObj);
                                }
                            }
                        }
                        else
                        {
                            // <== AMODS ==>
                            NotenizerDependency amod1 = sentence.Structure.GetDependencyByShortName(dependencyLoop, ComparisonType.DependentToGovernor, GrammaticalConstants.AdjectivalModifier);

                            // <== AMODS ==>
                            NotenizerDependency amod2 = sentence.Structure.GetDependencyByShortName(dependencyLoop, ComparisonType.GovernorToGovernor, GrammaticalConstants.AdjectivalModifier);

                            if (amod1 != null || amod2 != null)
                            {
                                if (amod1 != null)
                                {
                                    NoteParticle amod1Obj = new NoteParticle(amod1, TokenType.Dependent);
                                    notePart.Add(amod1Obj);
                                }

                                if (amod2 != null)
                                {
                                    NoteParticle amod2Obj = new NoteParticle(amod2, TokenType.Dependent);
                                    notePart.Add(amod2Obj);
                                }
                            }
                            else
                            {
                                // <== NUMMODS ==>
                                NotenizerDependency nummod1 = sentence.Structure.GetDependencyByShortName(dependencyLoop, ComparisonType.DependentToGovernor, GrammaticalConstants.NumericModifier);

                                // <== NUMMODS ==>
                                NotenizerDependency nummod2 = sentence.Structure.GetDependencyByShortName(dependencyLoop, ComparisonType.GovernorToGovernor, GrammaticalConstants.NumericModifier);

                                if (nummod1 != null)
                                {
                                    NoteParticle nummod1Obj = new NoteParticle(nummod1, TokenType.Dependent);
                                    notePart.Add(nummod1Obj);
                                }

                                if (nummod2 != null)
                                {
                                    NoteParticle nummod2Obj = new NoteParticle(nummod2, TokenType.Dependent);
                                    notePart.Add(nummod2Obj);
                                }
                            }
                        }

                        NoteParticle governorObj = new NoteParticle(dependencyLoop, TokenType.Governor);
                        notePart.Add(governorObj);
                    }
                    else if (POSConstants.VerbLikePOS.Contains(pos))
                    {

                        NoteParticle gov = new NoteParticle(dependencyLoop, TokenType.Governor);
                        notePart.Add(gov);

                        NotenizerDependency dobj = sentence.Structure.GetDependencyByShortName(dependencyLoop, ComparisonType.GovernorToGovernor, GrammaticalConstants.DirectObject);

                        if (dobj != null)
                        {
                            NoteParticle dobjObj = new NoteParticle(dobj, TokenType.Dependent);
                            notePart.Add(dobjObj);

                            NotenizerDependency neg = sentence.Structure.GetDependencyByShortName(dobj, ComparisonType.DependentToGovernor, GrammaticalConstants.NegationModifier);

                            if (neg != null)
                            {
                                NoteParticle negObj = new NoteParticle(neg, TokenType.Dependent);
                                notePart.Add(negObj);
                            }
                        }

                        NotenizerDependency aux = sentence.Structure.GetDependencyByShortName(
                            dependencyLoop,
                            ComparisonType.GovernorToGovernor,
                            GrammaticalConstants.AuxModifier,
                            GrammaticalConstants.AuxModifierPassive);

                        if (aux != null)
                        {
                            NoteParticle auxObj = new NoteParticle(aux, TokenType.Dependent);
                            notePart.Add(auxObj);
                        }

                        // <== NMODS ==>
                        List<NotenizerDependency> nmodsList = sentence.Structure.GetDependenciesByShortName(
                            dependencyLoop,
                            ComparisonType.GovernorToGovernor,
                            GrammaticalConstants.NominalModifier);

                        if (nmodsList != null && nmodsList.Count > 0)
                        {
                            NotenizerDependency first = nmodsList.First();
                            NotenizerDependency neg = sentence.Structure.GetDependencyByShortName(first, ComparisonType.DependentToGovernor, GrammaticalConstants.NegationModifier);

                            if (neg == null)
                            {
                                NoteParticle firstObj = new NoteParticle(first.Relation.AdjustedSpecific + NotenizerConstants.WordDelimeter + first.Dependent.Word, first, TokenType.Dependent);
                                notePart.Add(firstObj);
                            }
                            else
                            {
                                NoteParticle negObj = new NoteParticle(neg, TokenType.Dependent);
                                NoteParticle firstObj = new NoteParticle(first.Relation.AdjustedSpecific + NotenizerConstants.WordDelimeter + first.Dependent.Word, first, TokenType.Dependent);
                                notePart.Add(firstObj);
                                notePart.Add(negObj);
                            }

                            // second nmod depending on first one
                            NotenizerDependency nmodSecond = sentence.Structure.GetDependencyByShortName(first, ComparisonType.DependentToGovernor, GrammaticalConstants.NominalModifier);

                            if (nmodSecond != null)
                            {
                                neg = sentence.Structure.GetDependencyByShortName(first, ComparisonType.GovernorToGovernor, GrammaticalConstants.NegationModifier);

                                if (neg == null)
                                {
                                    NoteParticle secondObj = new NoteParticle(nmodSecond.Relation.AdjustedSpecific + NotenizerConstants.WordDelimeter + nmodSecond.Dependent.Word, nmodSecond, TokenType.Dependent);
                                    notePart.Add(secondObj);
                                }
                                else
                                {
                                    NoteParticle negObj = new NoteParticle(neg, TokenType.Dependent);
                                    NoteParticle secondObj = new NoteParticle(nmodSecond.Relation.AdjustedSpecific + NotenizerConstants.WordDelimeter + nmodSecond.Dependent.Word, nmodSecond, TokenType.Dependent);
                                    notePart.Add(secondObj);
                                    notePart.Add(negObj);
                                }
                            }
                        }
                    }
                    note.Add(notePart);
                }
            }

            return note;
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
        /// Filters dependencies by POS tag.
        /// </summary>
        /// <param name="dependencies"></param>
        /// <param name="poses"></param>
        /// <returns></returns>
        public List<NotenizerDependency> FilterByPOS(List<NotenizerDependency> dependencies, String[] poses)
        {
            List<NotenizerDependency> filteredDependencies = new List<NotenizerDependency>();

            foreach (NotenizerDependency dependencyLoop in dependencies)
            {
                if (poses.Contains(dependencyLoop.Dependent.POS.Tag))
                    filteredDependencies.Add(dependencyLoop);
            }

            return filteredDependencies;
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
            note.AndParserRule = andParserRule;
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
                NoteParticle dependencyObj = new NoteParticle(dependency, rule.TokenType);
                notePart.Add(dependencyObj);

                //if (dependency.Relation.IsNominalSubject())
                //{
                //    NoteParticle govObj = new NoteParticle(dependency, TokenType.Governor);
                //    notePart.Add(govObj);
                //}
            }
        }

        private List<NotenizerNote> AndParserFn(Annotation annotation)
        {
            AndParser andParser = new AndParser();

            List<NotenizerNote> notes = new List<NotenizerNote>();
            List<NotenizerNote> andNotes = new List<NotenizerNote>();

            NoteParticle processAndConjuctionsStartingParticle = null;
            List<NotenizerDependency> processAndConjuctionsConjuctions = null;

            foreach (Annotation sentenceLoop in annotation.get(typeof(CoreAnnotations.SentencesAnnotation)) as ArrayList)
            {
                notes.Add(StaticParser(new NotenizerSentence(sentenceLoop)));
            }

            foreach(NotenizerNote noteLoop in notes)
            {
                if (!noteLoop.OriginalSentence.Structure.CompressedDependencies.ContainsKey(GrammaticalConstants.Conjuction)
                    || noteLoop.OriginalSentence.Structure.CompressedDependencies[GrammaticalConstants.Conjuction] == null
                    || !noteLoop.OriginalSentence.Structure.CompressedDependencies[GrammaticalConstants.Conjuction].Any(x => x.Relation.Specific == GrammaticalConstants.AndConjuction))
                    continue;
                andParser.GetAndSets(noteLoop.OriginalSentence);
                NotenizerNote note = new NotenizerNote(noteLoop.OriginalSentence);

                foreach(NotePart notePartLoop in noteLoop.NoteParts)
                {
                    NotePart notePart = new NotePart(noteLoop.OriginalSentence);
                    processAndConjuctionsStartingParticle = null;
                    processAndConjuctionsConjuctions = null;

                    foreach(NoteParticle noteParticleLoop in notePartLoop.InitializedNoteParticles)
                    {
                        // preachadzat vsetky dependencies v note particle
                        // ak ani gov ani dep nemamju previazanie na ziadnu dependency so vztahoj conj:and
                        // tak ju pridaj do NotePart
                        //List<NotenizerDependency> depToGov = noteLoop.OriginalSentence.GetDependenciesByShortName(noteParticleLoop.NoteDependency, ComparisonType.DependantToGovernor, GrammaticalConstants.Conjuction);
                        //List<NotenizerDependency> govToGov = noteLoop.OriginalSentence.GetDependenciesByShortName(noteParticleLoop.NoteDependency, ComparisonType.GovernorToGovernor, GrammaticalConstants.Conjuction);
                        //List<NotenizerDependency> conjuctions = depToGov.Concat(govToGov).ToList();
                        List<NotenizerDependency> conjuctions = noteLoop.OriginalSentence.Structure.GetDependenciesByShortName(noteParticleLoop.NoteDependency, 
                            NotenizerExtensions.CreateComperisonType(noteParticleLoop.NoteDependency.TokenType, TokenType.Governor), 
                            GrammaticalConstants.Conjuction, GrammaticalConstants.AppositionalModifier);
                        
                        if (!conjuctions.Any(x => x.Relation.Specific == GrammaticalConstants.AndConjuction))
                        {
                            notePart.Add(noteParticleLoop);
                        }
                        else
                        {
                            processAndConjuctionsStartingParticle = noteParticleLoop;
                            processAndConjuctionsConjuctions = conjuctions;

                            break;
                        }

                        // ak ma, naklonuj NotePart,
                        // pridaj ju do povodnej NotePart a zisti dalsie prepojenie ako mnod:in a ukonci tak tento NotePart
                        // potom pre vsetky prepojenie s dependecy conj:and
                        // naklonuj NotePart
                        // zisti prepojenia ako nmod:in
                        // pridaj do naklonovaje NotePart
                        //
                        // nakoniec pridaj vsetky NoteParts do Note a tym vytvor novu poznamku 
                        // zlozenu z niekolkych viet, ktore budu mat rovnaky zaklad, ale budu
                        // sa lisit v prepojeniach AND
                    }

                    if (processAndConjuctionsStartingParticle != null)
                    {
                        NotePart clonedNotePart;
                        List<NotePart> clonedNoteParts = new List<NotePart>();
                        List<NotenizerDependency> repetitionPartDependencies = new List<NotenizerDependency>();
                        List<NotenizerDependency> andConjuctionsAppos = processAndConjuctionsConjuctions.Where(x => x.Relation.Specific == GrammaticalConstants.AndConjuction
                            || x.Relation.ShortName == GrammaticalConstants.AppositionalModifier).ToList();

                        repetitionPartDependencies = repetitionPartDependencies.Concat(andConjuctionsAppos).ToList();

                        foreach (NotenizerDependency andApposDependencyLoop in andConjuctionsAppos)
                        {
                            repetitionPartDependencies = repetitionPartDependencies.Concat(
                                noteLoop.OriginalSentence.Structure.GetDependenciesByShortName(andApposDependencyLoop, ComparisonType.DependentToGovernor, GrammaticalConstants.AppositionalModifier)).ToList();
                        }

                        foreach (NotenizerDependency andConjuctionLoop in repetitionPartDependencies)
                        {
                            clonedNotePart = notePart.Clone();
                            clonedNoteParts.Add(clonedNotePart);
                            clonedNotePart.Add(new NoteParticle(andConjuctionLoop.Dependent, andConjuctionLoop));

                            AddAditionalNoteParticles(noteLoop.OriginalSentence, andConjuctionLoop, clonedNotePart);
                        }

                        notePart.Add(new NoteParticle(processAndConjuctionsStartingParticle.NoteDependency.CorrespondingWord, processAndConjuctionsStartingParticle.NoteDependency));
                        AddAditionalNoteParticles(noteLoop.OriginalSentence, processAndConjuctionsStartingParticle.NoteDependency, notePart);
                        note.Add(notePart);
                        note.Add(clonedNoteParts);
                        andNotes.Add(note);
                    }
                }
            }

            return andNotes;
        }

        public void AddAditionalNoteParticles(NotenizerSentence sentence, NotenizerDependency dependency, NotePart destinationNotePart)
        {
            NotenizerDependency compound = sentence.Structure.GetDependencyByShortName(dependency, ComparisonType.DependentToGovernor, GrammaticalConstants.CompoudModifier);
            if (compound != null)
                destinationNotePart.Add(new NoteParticle(compound.Dependent, compound));

            NotenizerDependency nmod = sentence.Structure.GetDependencyByShortName(dependency, ComparisonType.DependentToGovernor, GrammaticalConstants.NominalModifier);
            if (nmod != null)
                destinationNotePart.Add(new NoteParticle(nmod.Dependent, nmod));
        }

        private bool IsAndParsableSentence(NotenizerSentence sentence)
        {
            return ((sentence.Structure.CompressedDependencies.ContainsKey(GrammaticalConstants.Conjuction)
                && sentence.Structure.CompressedDependencies[GrammaticalConstants.Conjuction] != null
                && sentence.Structure.CompressedDependencies[GrammaticalConstants.Conjuction].Any(x => x.Relation.Specific == GrammaticalConstants.AndConjuction))
                || sentence.Structure.Dependencies.Where(x => x.Dependent.Word.ToLower().Trim() == GrammaticalConstants.AndConjuction || x.Governor.Word.ToLower() == GrammaticalConstants.AndConjuction).Count() > 0);
        }
    }
}
