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

namespace nsNotenizer
{
    public class Notenizer
    {
		public Notenizer()
		{

		}

        public void RunCoreNLP(String text = null)
        {
            var jarRoot = @"stanford-corenlp-3.5.2-models";

            // Text for processing
            if (text == null)
                text = "Kosgi Santosh sent an email to Stanford University. He didn't get a reply.";

            // Annotation pipeline configuration
            var props = new Properties();
            //props.setProperty("annotators", "tokenize, ssplit, pos, lemma, ner, parse, dcoref");
            props.setProperty("annotators", "tokenize, ssplit, pos, parse");
            props.setProperty("sutime.binders", "0");
            props.setProperty("ner.useSUTime", "false");

            // bugfix for time exception
            CultureInfo cultureInfo = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            // We should change current directory, so StanfordCoreNLP could find all the model files automatically
            String curDir = Environment.CurrentDirectory;
            Directory.SetCurrentDirectory(jarRoot);
            StanfordCoreNLP pipeline = new StanfordCoreNLP(props);
            Directory.SetCurrentDirectory(curDir);

            // Annotation
            Annotation annotation = new Annotation(text);
            pipeline.annotate(annotation);

            // Result - Pretty Print
            using (ByteArrayOutputStream stream = new ByteArrayOutputStream())
            {
                pipeline.prettyPrint(annotation, new PrintWriter(stream));
                Console.WriteLine(stream.toString());
                stream.close();
            }

            Console.WriteLine("======================================================");

            // these are all the sentences in this document
            // a CoreMap is essentially a Map that uses class objects as keys and has values with custom types
            //var sentences = annotation.get(typeof(CoreAnnotations.SentencesAnnotation));
            //if (sentences == null)
            //{
            //	return;
            //}

            //foreach (Annotation sentence in sentences as ArrayList)
            //{
            //	Console.WriteLine(sentence);
            //}

            //Test(annotation);
            PrintNotes(Parse(annotation));
        }

        private void Test(Annotation annotation)
        {

            String firstPart = String.Empty;
            String midPart = String.Empty;
            String endPart = String.Empty;
            //String note = String.Empty;
            String nmods = String.Empty;
            Dictionary<String, String> sentencesNoted = new Dictionary<String, String>();
            List<Object> poses = new List<Object>();

            foreach (Annotation sentence in annotation.get(typeof(CoreAnnotations.SentencesAnnotation)) as ArrayList)
            {
                // getting dependencies
                //Annotation firstSentence = (annotation.get(typeof(CoreAnnotations.SentencesAnnotation)) as ArrayList).get(1) as Annotation;
                Tree tree = sentence.get(typeof(TreeCoreAnnotations.TreeAnnotation)) as Tree;
                TreebankLanguagePack treeBankLangPack = new PennTreebankLanguagePack();
                GrammaticalStructureFactory gramStructFact = treeBankLangPack.grammaticalStructureFactory();
                GrammaticalStructure gramStruct = gramStructFact.newGrammaticalStructure(tree);
                Collection typedDependencies = gramStruct.typedDependenciesCollapsed();
                Console.WriteLine(typedDependencies);

                List<TypedDependency> list = (typedDependencies as ArrayList).ToList<TypedDependency>();
                Console.WriteLine(list.Count);

                foreach (TypedDependency typedDependency in list)
                {
                    //Console.WriteLine("Dependancy name " + (typedDependency.dep() as IndexedWord) + " NODE " + typedDependency.reln());
                    poses.Add(typedDependency.gov().get(typeof(CoreAnnotations.PartOfSpeechAnnotation)));
                    poses.Add(typedDependency.dep().get(typeof(CoreAnnotations.PartOfSpeechAnnotation)));
                    if (typedDependency.reln().IsGrammaticalRelation(EnglishGrammaticalRelations.NOMINAL_SUBJECT)
                        || typedDependency.reln().IsGrammaticalRelation(EnglishGrammaticalRelations.NOMINAL_PASSIVE_SUBJECT))
                    {
                        firstPart = typedDependency.dep().word();

                        String pos = typedDependency.gov().get(typeof(CoreAnnotations.PartOfSpeechAnnotation)).ToString();
                        if (pos == "NN" || pos == "NNP" || pos == "JJR" || pos == "JJ" || pos == "CD")
                        {
                            TypedDependency compound = list
                                .Where(x => x.reln().getLongName() == "compound modifier")
                                .Where(y => y.gov().GetUniqueIdentifier() == typedDependency.dep().GetUniqueIdentifier())
                                .Select(z => z).FirstOrDefault();

                            firstPart = compound != null ? compound.dep().word() + " " + firstPart : firstPart;

                            TypedDependency aux = list
                                .Where(x => x.reln().IsGrammaticalRelation(EnglishGrammaticalRelations.AUX_MODIFIER) || x.reln().IsGrammaticalRelation(EnglishGrammaticalRelations.AUX_PASSIVE_MODIFIER))
                                .Where(y => y.gov().GetUniqueIdentifier() == typedDependency.gov().GetUniqueIdentifier())
                                .Select(z => z).FirstOrDefault();

                            if (aux != null)
                                firstPart += " " + aux.dep().word();

                            TypedDependency cop = list
                                .Where(x => x.reln().IsGrammaticalRelation(EnglishGrammaticalRelations.COPULA))
                                .Where(y => y.gov().GetUniqueIdentifier() == typedDependency.gov().GetUniqueIdentifier())
                                .Select(z => z).FirstOrDefault();

                            List<TypedDependency> conjuctions = list
                                .Where(x => x.reln().getLongName() == "conj_collapsed")
                                .Where(y => y.gov().GetUniqueIdentifier() == typedDependency.gov().GetUniqueIdentifier())
                                .Select(z => z).ToList();

                            String conjs = String.Empty;
                            String specific = String.Empty;
                            if (conjuctions != null && conjuctions.Count > 0)
                            {
                                specific = " " + conjuctions.First().reln().getSpecific() + " ";
                                List<TypedDependency> filteredConjs = conjuctions.FilterByPOS(new String[] { "NN", "NNP", "JJ", "JJR" });

                                if (filteredConjs.Count > 0)
                                    conjs = specific + String.Join(specific, filteredConjs.Select(x => x.dep().word()));
                            }

                            midPart += cop.dep().word();

                            // <== NMODS ==>
                            List<TypedDependency> nmodsList = list
                                .Where(x => x.reln().getShortName() == "nmod")
                                .Where(y => y.gov().GetUniqueIdentifier() == typedDependency.gov().GetUniqueIdentifier())
                                .Select(z => z).ToList();

                            if (nmodsList != null && nmodsList.Count > 0)
                            {
                                TypedDependency neg = list
                                    .Where(x => x.reln().IsGrammaticalRelation(EnglishGrammaticalRelations.NEGATION_MODIFIER))
                                    .Where(y => y.gov().GetUniqueIdentifier() == nmodsList.First().dep().GetUniqueIdentifier())
                                    .Select(z => z).FirstOrDefault();

                                nmods += " ";
                                nmods = neg == null
                                    ? nmodsList.First().reln().GetAdjustedSpecific() + " " + nmodsList.First().dep().word().ToString()
                                    : nmodsList.First().reln().GetAdjustedSpecific() + " " + neg.dep().word().ToString() + " " + nmodsList.First().dep().word().ToString();

                                // second nmod depending on first one
                                TypedDependency nmodSecond = list
                                    .Where(x => x.reln().getShortName() == "nmod")
                                    .Where(y => y.gov().GetUniqueIdentifier() == nmodsList.First().dep().GetUniqueIdentifier())
                                    .Select(z => z).FirstOrDefault();

                                if (nmodSecond != null)
                                {

                                    neg = list
                                        .Where(x => x.reln().IsGrammaticalRelation(EnglishGrammaticalRelations.NEGATION_MODIFIER))
                                        .Where(y => y.gov().GetUniqueIdentifier() == nmodsList.First().gov().GetUniqueIdentifier())
                                        .Select(z => z).FirstOrDefault();

                                    nmods += " ";
                                    nmods += neg == null
                                        ? nmodSecond.reln().GetAdjustedSpecific() + " " + nmodSecond.dep().word().ToString()
                                        : nmodSecond.reln().GetAdjustedSpecific() + " " + neg.dep().word().ToString() + " " + nmodSecond.dep().word().ToString();
                                }
                            }
                            else
                            {
                                // <== AMODS ==>
                                TypedDependency amod1 = list
                                    .Where(x => x.reln().getShortName() == "amod")
                                    .Where(y => y.gov().GetUniqueIdentifier() == typedDependency.dep().GetUniqueIdentifier())
                                    .Select(z => z).FirstOrDefault();

                                // <== AMODS ==>
                                TypedDependency amod2 = list
                                    .Where(x => x.reln().getShortName() == "amod")
                                    .Where(y => y.gov().GetUniqueIdentifier() == typedDependency.gov().GetUniqueIdentifier())
                                    .Select(z => z).FirstOrDefault();

                                if (amod1 != null || amod2 != null)
                                {
                                    if (amod1 != null)
                                        firstPart = firstPart.Preppend(amod1.dep().word());

                                    if (amod2 != null)
                                        midPart += " " + amod2.dep().word();
                                }
                                else
                                {
                                    // <== NUMMODS ==>
                                    TypedDependency nummod1 = list
                                        .Where(x => x.reln().getShortName() == "nummod")
                                        .Where(y => y.gov().GetUniqueIdentifier() == typedDependency.dep().GetUniqueIdentifier())
                                        .Select(z => z).FirstOrDefault();

                                    // <== NUMMODS ==>
                                    TypedDependency nummod2 = list
                                        .Where(x => x.reln().getShortName() == "nummod")
                                        .Where(y => y.gov().GetUniqueIdentifier() == typedDependency.gov().GetUniqueIdentifier())
                                        .Select(z => z).FirstOrDefault();

                                    if (nummod1 != null)
                                        firstPart = firstPart.Preppend(nummod1.dep().word());

                                    if (nummod2 != null)
                                        midPart += " " + nummod2.dep().word();
                                }
                            }

                            endPart = typedDependency.gov().word() + conjs + " " + nmods;
                        }
                        else if (pos == "VB" || pos == "VBN" || pos == "VBG" || pos == "VBD" || pos == "VBP" || pos == "VBZ")
                        {
                            midPart = typedDependency.gov().word();

                            TypedDependency dobj = list
                                .Where(x => x.reln().IsGrammaticalRelation(EnglishGrammaticalRelations.DIRECT_OBJECT))
                                .Where(y => y.gov().GetUniqueIdentifier() == typedDependency.gov().GetUniqueIdentifier())
                                .Select(z => z).FirstOrDefault();

                            TypedDependency aux = list
                                .Where(x => x.reln().IsGrammaticalRelation(EnglishGrammaticalRelations.AUX_MODIFIER) || x.reln().IsGrammaticalRelation(EnglishGrammaticalRelations.AUX_PASSIVE_MODIFIER))
                                .Where(y => y.gov().GetUniqueIdentifier() == typedDependency.gov().GetUniqueIdentifier())
                                .Select(z => z).FirstOrDefault();

                            // <== NMODS ==>
                            List<TypedDependency> nmodsList = list
                                .Where(x => x.reln().getShortName() == "nmod")
                                .Where(y => y.gov().GetUniqueIdentifier() == typedDependency.gov().GetUniqueIdentifier())
                                .Select(z => z).ToList();

                            if (nmodsList != null && nmodsList.Count > 0)
                            {
                                TypedDependency neg = list
                                    .Where(x => x.reln().IsGrammaticalRelation(EnglishGrammaticalRelations.NEGATION_MODIFIER))
                                    .Where(y => y.gov().GetUniqueIdentifier() == nmodsList.First().dep().GetUniqueIdentifier())
                                    .Select(z => z).FirstOrDefault();

                                nmods += " ";
                                nmods = neg == null
                                    ? nmodsList.First().reln().GetAdjustedSpecific() + " " + nmodsList.First().dep().word().ToString()
                                    : nmodsList.First().reln().GetAdjustedSpecific() + " " + neg.dep().word().ToString() + " " + nmodsList.First().dep().word().ToString();

                                // second nmod depending on first one
                                TypedDependency nmodSecond = list
                                    .Where(x => x.reln().getShortName() == "nmod")
                                    .Where(y => y.gov().GetUniqueIdentifier() == nmodsList.First().dep().GetUniqueIdentifier())
                                    .Select(z => z).FirstOrDefault();

                                if (nmodSecond != null)
                                {

                                    neg = list
                                        .Where(x => x.reln().IsGrammaticalRelation(EnglishGrammaticalRelations.NEGATION_MODIFIER))
                                        .Where(y => y.gov().GetUniqueIdentifier() == nmodsList.First().gov().GetUniqueIdentifier())
                                        .Select(z => z).FirstOrDefault();

                                    nmods += " ";
                                    nmods += neg == null
                                        ? nmodSecond.reln().GetAdjustedSpecific() + " " + nmodSecond.dep().word().ToString()
                                        : nmodSecond.reln().GetAdjustedSpecific() + " " + neg.dep().word().ToString() + " " + nmodSecond.dep().word().ToString();
                                }
                            }

                            if (aux != null)
                                midPart = aux.dep().word() + " " + midPart;

                            if (dobj != null)
                                endPart += dobj.dep().word();

                            endPart += " " + nmods;
                        }

                    }
                    // typedDependency contains functions dep() and gov() which return IndexedWord which has function like lemma(), new(), and so on..
                    // typedDependency alson ocntains reln() wich is a relation (like nsubjpass, ...)
                }
                sentencesNoted.Add(sentence.toString(), firstPart + " " + midPart + " " + endPart);
                firstPart = midPart = endPart = nmods = String.Empty;
            }
            //PrintNotes(sentencesNoted);

        }

        public List<Note> Parse(Annotation annotation)
        {
            List<Note> sentencesNoted = new List<Note>();

            // ================== REFACTORED PART HERE ======================
            foreach (Annotation sentenceLoop in annotation.get(typeof(CoreAnnotations.SentencesAnnotation)) as ArrayList)
            {
                NotePart firstPartNotePart = new NotePart();
                NotePart mainPartNotePart = new NotePart();
                NotePart lastPartNotePart = new NotePart();
                NotenizerSentence sentence = new NotenizerSentence(sentenceLoop);
                Note note = new Note(sentence);
                Note noteLoop;

                //NotenizerRule rule = DocumentParser.ParseNoteDependencies(
                //    DB.GetFirst(DBConstants.NotesCollectionName, DocumentCreator.CreateFilterByDependencies(sentence)).Result);

                NotenizerRule rule = DocumentParser.GetHeighestMatch(sentence,
                    DB.GetAll(DBConstants.NotesCollectionName, DocumentCreator.CreateFilterByDependencies(sentence)).Result);

                if (rule.RuleDependencies != null && rule.RuleDependencies.Count > 0)
                {
                    Note parsedNote = ApplyRule(sentence, rule);
                    Console.WriteLine("Parsed note: " + parsedNote.OriginalSentence + " ===> " + parsedNote.Value);

                    continue;
                }

                foreach (NotenizerDependency dependencyLoop in sentence.Dependencies)
                {
                    if (dependencyLoop.Relation.IsRelation(GrammaticalConstants.NominalSubject)
                        || dependencyLoop.Relation.IsRelation(GrammaticalConstants.NominalSubjectPassive))
                    {
                        NotePart notePart = new NotePart(sentence);

                        noteLoop = new Note();

                        NoteObject nsubj = new NoteObject(dependencyLoop.Dependent.Word, dependencyLoop.Dependent, dependencyLoop);
                        notePart.Add(nsubj);

                        String pos = dependencyLoop.Governor.POS;
                        if (POSConstants.NounLikePOS.Contains(pos))
                        {
                            NotenizerDependency compound = sentence.GetDependencyByShortName(
                                dependencyLoop,
                                ComparisonType.DependantToGovernor,
                                GrammaticalConstants.CompoudModifier);

                            if (compound != null)
                            {
                                NoteObject compoundObj = new NoteObject(compound.Dependent.Word, compound.Dependent, compound);
                                notePart.Add(compoundObj);
                            }

                            NotenizerDependency aux = sentence.GetDependencyByShortName(
                                dependencyLoop,
                                ComparisonType.GovernorToGovernor,
                                GrammaticalConstants.AuxModifier,
                                GrammaticalConstants.AuxModifierPassive);

                            if (aux != null)
                            {
                                NoteObject auxObj = new NoteObject(aux.Dependent.Word, aux.Dependent, aux);
                                notePart.Add(auxObj);
                            }

                            NotenizerDependency cop = sentence.GetDependencyByShortName(dependencyLoop, ComparisonType.GovernorToGovernor, GrammaticalConstants.Copula);

                            if (cop != null)
                            {
                                NoteObject copObj = new NoteObject(cop.Dependent.Word, cop.Dependent, cop);
                                notePart.Add(copObj);
                            }

                            List<NotenizerDependency> conjuctions = sentence.GetDependenciesByShortName(
                                dependencyLoop,
                                ComparisonType.GovernorToGovernor,
                                GrammaticalConstants.Conjuction);


                            String specific = String.Empty;
                            if (conjuctions != null && conjuctions.Count > 0)
                            {
                                List<NotenizerDependency> filteredConjs = FilterByPOS(conjuctions, POSConstants.ConjustionPOS);

                                foreach (NotenizerDependency filteredConjLoop in filteredConjs)
                                {
                                    NotenizerDependency cc = sentence.GetDependencyByShortName(dependencyLoop, ComparisonType.GovernorToGovernor, GrammaticalConstants.CoordinatingConjuction);

                                    if (cc.Dependent.Word == filteredConjLoop.Relation.Specific
                                        && sentence.DependencyIndex(filteredConjLoop) > sentence.DependencyIndex(cc))
                                    {
                                        NoteObject ccObj = new NoteObject(cc.Dependent.Word, cc.Dependent, cc);
                                        NoteObject filteredConjObj = new NoteObject(filteredConjLoop.Dependent.Word, filteredConjLoop.Dependent, filteredConjLoop);

                                        notePart.Add(ccObj);
                                        notePart.Add(filteredConjObj);
                                    }
                                }
                            }

                            // <== NMODS ==>
                            List<NotenizerDependency> nmodsList = sentence.GetDependenciesByShortName(
                                dependencyLoop, ComparisonType.GovernorToGovernor, GrammaticalConstants.NominalModifier);

                            if (nmodsList != null && nmodsList.Count > 0)
                            {
                                NotenizerDependency neg = sentence.GetDependencyByShortName(
                                    nmodsList.First(), ComparisonType.DependantToGovernor, GrammaticalConstants.NegationModifier);

                                if (neg == null)
                                {
                                    NoteObject firstObj = new NoteObject(nmodsList.First().Relation.AdjustedSpecific + NotenizerConstants.WordDelimeter + nmodsList.First().Dependent.Word, nmodsList.First().Dependent, nmodsList.First());
                                    notePart.Add(firstObj);
                                }
                                else
                                {
                                    NoteObject negObj = new NoteObject(neg.Dependent.Word, neg.Dependent, neg);
                                    NoteObject firstObj = new NoteObject(nmodsList.First().Relation.AdjustedSpecific + NotenizerConstants.WordDelimeter + nmodsList.First().Dependent.Word, nmodsList.First().Dependent, nmodsList.First());

                                    notePart.Add(negObj);
                                    notePart.Add(firstObj);
                                }

                                // second nmod depending on first one
                                NotenizerDependency nmodSecond = sentence.GetDependencyByShortName(
                                    nmodsList.First(), ComparisonType.DependantToGovernor, GrammaticalConstants.NominalModifier);

                                if (nmodSecond != null)
                                {
                                    neg = sentence.GetDependencyByShortName(
                                    nmodsList.First(), ComparisonType.GovernorToGovernor, GrammaticalConstants.NegationModifier);

                                    if (neg == null)
                                    {
                                        NoteObject secondObj = new NoteObject(nmodSecond.Relation.AdjustedSpecific + NotenizerConstants.WordDelimeter + nmodSecond.Dependent.Word, nmodSecond.Dependent, nmodSecond);
                                        notePart.Add(secondObj);
                                    }
                                    else
                                    {
                                        NoteObject negObj = new NoteObject(neg.Dependent.Word, neg.Dependent, neg);
                                        NoteObject secondObj = new NoteObject(nmodSecond.Relation.AdjustedSpecific + NotenizerConstants.WordDelimeter + nmodSecond.Dependent.Word, nmodSecond.Dependent, nmodSecond);

                                        notePart.Add(negObj);
                                        notePart.Add(secondObj);
                                    }
                                }
                            }
                            else
                            {
                                // <== AMODS ==>
                                NotenizerDependency amod1 = sentence.GetDependencyByShortName(dependencyLoop, ComparisonType.DependantToGovernor, GrammaticalConstants.AdjectivalModifier);

                                // <== AMODS ==>
                                NotenizerDependency amod2 = sentence.GetDependencyByShortName(dependencyLoop, ComparisonType.GovernorToGovernor, GrammaticalConstants.AdjectivalModifier);

                                if (amod1 != null || amod2 != null)
                                {
                                    if (amod1 != null)
                                    {
                                        NoteObject amod1Obj = new NoteObject(amod1.Dependent.Word, amod1.Dependent, amod1);
                                        notePart.Add(amod1Obj);
                                    }

                                    if (amod2 != null)
                                    {
                                        NoteObject amod2Obj = new NoteObject(amod2.Dependent.Word, amod2.Dependent, amod2);
                                        notePart.Add(amod2Obj);
                                    }
                                }
                                else
                                {
                                    // <== NUMMODS ==>
                                    NotenizerDependency nummod1 = sentence.GetDependencyByShortName(dependencyLoop, ComparisonType.DependantToGovernor, GrammaticalConstants.NumericModifier);

                                    // <== NUMMODS ==>
                                    NotenizerDependency nummod2 = sentence.GetDependencyByShortName(dependencyLoop, ComparisonType.GovernorToGovernor, GrammaticalConstants.NumericModifier);

                                    if (nummod1 != null)
                                    {
                                        NoteObject nummod1Obj = new NoteObject(nummod1.Dependent.Word, nummod1.Dependent, nummod1);
                                        notePart.Add(nummod1Obj);
                                    }

                                    if (nummod2 != null)
                                    {
                                        NoteObject nummod2Obj = new NoteObject(nummod2.Dependent.Word, nummod2.Dependent, nummod2);
                                        notePart.Add(nummod2Obj);
                                    }
                                }
                            }

                            NoteObject governorObj = new NoteObject(dependencyLoop.Governor.Word, dependencyLoop.Governor, dependencyLoop);
                            notePart.Add(governorObj);
                        }
                        else if (POSConstants.VerbLikePOS.Contains(pos))
                        {
                            noteLoop = new Note();

                            NoteObject gov = new NoteObject(dependencyLoop.Governor.Word, dependencyLoop.Governor, dependencyLoop);
                            notePart.Add(gov);

                            NotenizerDependency dobj = sentence.GetDependencyByShortName(dependencyLoop, ComparisonType.GovernorToGovernor, GrammaticalConstants.DirectObject);

                            if (dobj != null)
                            {
                                NoteObject dobjObj = new NoteObject(dobj.Dependent.Word, dobj.Dependent, dobj);
                                notePart.Add(dobjObj);

                                NotenizerDependency neg = sentence.GetDependencyByShortName(dobj, ComparisonType.DependantToGovernor, GrammaticalConstants.NegationModifier);

                                if (neg != null)
                                {
                                    NoteObject negObj = new NoteObject(neg.Dependent.Word, neg.Dependent, neg);
                                    notePart.Add(negObj);
                                }
                            }

                            NotenizerDependency aux = sentence.GetDependencyByShortName(
                                dependencyLoop,
                                ComparisonType.GovernorToGovernor,
                                GrammaticalConstants.AuxModifier,
                                GrammaticalConstants.AuxModifierPassive);

                            if (aux != null)
                            {
                                NoteObject auxObj = new NoteObject(aux.Dependent.Word, aux.Dependent, aux);
                                notePart.Add(auxObj);
                            }

                            // <== NMODS ==>
                            List<NotenizerDependency> nmodsList = sentence.GetDependenciesByShortName(
                                dependencyLoop,
                                ComparisonType.GovernorToGovernor,
                                GrammaticalConstants.NominalModifier);

                            if (nmodsList != null && nmodsList.Count > 0)
                            {
                                NotenizerDependency neg = sentence.GetDependencyByShortName(nmodsList.First(), ComparisonType.DependantToGovernor, GrammaticalConstants.NegationModifier);

                                if (neg == null)
                                {
                                    NoteObject firstObj = new NoteObject(nmodsList.First().Relation.AdjustedSpecific + NotenizerConstants.WordDelimeter + nmodsList.First().Dependent.Word, nmodsList.First().Dependent, nmodsList.First());
                                    notePart.Add(firstObj);
                                }
                                else
                                {
                                    NoteObject negObj = new NoteObject(neg.Dependent.Word, neg.Dependent, neg);
                                    NoteObject firstObj = new NoteObject(nmodsList.First().Relation.AdjustedSpecific + NotenizerConstants.WordDelimeter + nmodsList.First().Dependent.Word, nmodsList.First().Dependent, nmodsList.First());
                                    notePart.Add(firstObj);
                                    notePart.Add(negObj);
                                }

                                // second nmod depending on first one
                                NotenizerDependency nmodSecond = sentence.GetDependencyByShortName(nmodsList.First(), ComparisonType.DependantToGovernor, GrammaticalConstants.NominalModifier);

                                if (nmodSecond != null)
                                {
                                    neg = sentence.GetDependencyByShortName(nmodsList.First(), ComparisonType.GovernorToGovernor, GrammaticalConstants.NegationModifier);

                                    if (neg == null)
                                    {
                                        NoteObject secondObj = new NoteObject(nmodSecond.Relation.AdjustedSpecific + NotenizerConstants.WordDelimeter + nmodSecond.Dependent.Word, nmodSecond.Dependent, nmodSecond);
                                        notePart.Add(secondObj);
                                    }
                                    else
                                    {
                                        NoteObject negObj = new NoteObject(neg.Dependent.Word, neg.Dependent, neg);
                                        NoteObject secondObj = new NoteObject(nmodSecond.Relation.AdjustedSpecific + NotenizerConstants.WordDelimeter + nmodSecond.Dependent.Word, nmodSecond.Dependent, nmodSecond);
                                        notePart.Add(secondObj);
                                        notePart.Add(negObj);
                                    }
                                }
                            }
                        }
                        note.Add(notePart);
                    }
                }
                //String _id = DB.InsertToCollection("notes", DocumentCreator.CreateNoteDocument(note, -1)).Result;
                sentencesNoted.Add(note);
            }

            return sentencesNoted;
        }

        public void PrintNotes(List<Note> notes)
        {
            Console.WriteLine();
            Console.WriteLine("<======== NOTES ========>");

            foreach (Note noteLoop in notes)
            {
                Console.WriteLine(noteLoop.OriginalSentence + " ---> " + noteLoop.Value);
            }
        }

        public List<NotenizerDependency> FilterByPOS(List<NotenizerDependency> dependencies, String[] poses)
        {
            List<NotenizerDependency> filteredDependencies = new List<NotenizerDependency>();

            foreach (NotenizerDependency dependencyLoop in dependencies)
            {
                if (poses.Contains(dependencyLoop.Dependent.POS))
                    filteredDependencies.Add(dependencyLoop);
            }

            return filteredDependencies;
        }

        private Note ApplyRule(NotenizerSentence sentence, NotenizerRule rule)
        {
            Note note = new Note(sentence);
            NotePart notePart = new NotePart(sentence);

            foreach (NotenizerDependency ruleLoop in rule.RuleDependencies)
            {
                ApplyRule(sentence, ruleLoop, notePart);
            }

            note.Add(notePart);

            note.SplitToSentences(rule.SentencesEnds);

            return note;
        }

        private void ApplyRule(NotenizerSentence sentence, NotenizerDependency rule, NotePart notePart)
        {
            NotenizerDependency dependency = sentence.FindDependency(rule);

            if (dependency != null)
            {
                NoteObject dependencyObj = new NoteObject(dependency.Dependent, dependency);
                notePart.Add(dependencyObj);

                if (dependency.Relation.IsNominalSubject())
                {
                    NoteObject govObj = new NoteObject(dependency.Governor, dependency);
                    notePart.Add(govObj);
                }
            }
        }
    }
}
