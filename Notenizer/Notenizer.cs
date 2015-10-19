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
            bool findCOP = false;
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

                foreach (NotenizerDependency dependencyLoop in sentence.Dependencies)
                {
                    if (dependencyLoop.Relation.IsRelation(GrammaticalConstants.NominalSubject)
                        || dependencyLoop.Relation.IsRelation(GrammaticalConstants.NominalSubjectPassive))
                    {
                        noteLoop = new Note();

                        firstPartNotePart.MainWord = dependencyLoop.Dependant.Value;

                        String pos = dependencyLoop.Governor.POS;
                        if (POSConstants.NounLikePOS.Contains(pos))
                        {
                            NotenizerDependency compound = sentence.GetDependencyByShortName(
                                dependencyLoop,
                                Comparison.DependantToGovernor,
                                GrammaticalConstants.CompoudModifier);

                            if (compound != null)
                                firstPartNotePart.PrefixWord = compound.Dependant.Value;

                            NotenizerDependency aux = sentence.GetDependencyByShortName(
                                dependencyLoop,
                                Comparison.GovernorToGovernor,
                                GrammaticalConstants.AuxModifier,
                                GrammaticalConstants.AuxModifierPassive);

                            if (aux != null)
                                firstPartNotePart.PostfixWord = aux.Dependant.Value;

                            NotenizerDependency cop = sentence.GetDependencyByShortName(dependencyLoop, Comparison.GovernorToGovernor, GrammaticalConstants.Copula);

                            if (cop != null)
                                mainPartNotePart.MainWord = cop.Dependant.Value;

                            List<NotenizerDependency> conjuctions = sentence.GetDependenciesByShortName(
                                dependencyLoop,
                                Comparison.GovernorToGovernor,
                                GrammaticalConstants.Conjuction);


                            String specific = String.Empty;
                            if (conjuctions != null && conjuctions.Count > 0)
                            {
                                specific = NotenizerConstants.WordDelimeter + conjuctions.First().Relation.Specific + NotenizerConstants.WordDelimeter;
                                List<NotenizerDependency> filteredConjs = FilterByPOS(conjuctions, POSConstants.ConjustionPOS);

                                if (filteredConjs.Count > 0)
                                    lastPartNotePart.MainWord = String.Join(specific, filteredConjs.Select(x => x.Dependant.Value)) + specific;
                            }

                            // <== NMODS ==>
                            List<NotenizerDependency> nmodsList = sentence.GetDependenciesByShortName(
                                dependencyLoop, Comparison.GovernorToGovernor, GrammaticalConstants.NominalModifier);

                            if (nmodsList != null && nmodsList.Count > 0)
                            {
                                NotenizerDependency neg = sentence.GetDependencyByShortName(
                                    nmodsList.First(), Comparison.DependantToGovernor, GrammaticalConstants.NegationModifier);

                                if (neg == null)
                                {
                                    lastPartNotePart.PostfixWord =
                                        nmodsList.First().Relation.AdjustedSpecific + NotenizerConstants.WordDelimeter + nmodsList.First().Dependant.Value;
                                }
                                else
                                {
                                    lastPartNotePart.PostfixWord =
                                        nmodsList.First().Relation.AdjustedSpecific + NotenizerConstants.WordDelimeter + neg.Dependant.Value
                                        + NotenizerConstants.WordDelimeter + nmodsList.First().Dependant.Value;
                                }

                                // second nmod depending on first one
                                NotenizerDependency nmodSecond = sentence.GetDependencyByShortName(
                                    nmodsList.First(), Comparison.DependantToGovernor, GrammaticalConstants.NominalModifier);

                                if (nmodSecond != null)
                                {
                                    neg = sentence.GetDependencyByShortName(
                                    nmodsList.First(), Comparison.GovernorToGovernor, GrammaticalConstants.NegationModifier);

                                    if (neg == null)
                                    {
                                        lastPartNotePart.PostfixWord += NotenizerConstants.WordDelimeter +
                                            nmodSecond.Relation.AdjustedSpecific + NotenizerConstants.WordDelimeter + nmodSecond.Dependant.Value;
                                    }
                                    else
                                    {
                                        lastPartNotePart.PostfixWord += NotenizerConstants.WordDelimeter +
                                            nmodSecond.Relation.AdjustedSpecific + NotenizerConstants.WordDelimeter + neg.Dependant.Value
                                            + NotenizerConstants.WordDelimeter + nmodSecond.Dependant.Value;
                                    }
                                }
                            }
                            else
                            {
                                // <== AMODS ==>
                                NotenizerDependency amod1 = sentence.GetDependencyByShortName(dependencyLoop, Comparison.DependantToGovernor, GrammaticalConstants.AdjectivalModifier);

                                // <== AMODS ==>
                                NotenizerDependency amod2 = sentence.GetDependencyByShortName(dependencyLoop, Comparison.GovernorToGovernor, GrammaticalConstants.AdjectivalModifier);

                                if (amod1 != null || amod2 != null)
                                {
                                    if (amod1 != null)
                                        firstPartNotePart.PrefixWord = firstPartNotePart.PrefixWord.Preppend(amod1.Dependant.Value);

                                    if (amod2 != null)
                                        mainPartNotePart.PostfixWord = amod2.Dependant.Value;
                                }
                                else
                                {
                                    // <== NUMMODS ==>
                                    NotenizerDependency nummod1 = sentence.GetDependencyByShortName(dependencyLoop, Comparison.DependantToGovernor, GrammaticalConstants.NumericModifier);

                                    // <== NUMMODS ==>
                                    NotenizerDependency nummod2 = sentence.GetDependencyByShortName(dependencyLoop, Comparison.GovernorToGovernor, GrammaticalConstants.NumericModifier);

                                    if (nummod1 != null)
                                        firstPartNotePart.PrefixWord = firstPartNotePart.PrefixWord.Preppend(nummod1.Dependant.Value);

                                    if (nummod2 != null)
                                        mainPartNotePart.PostfixWord = nummod2.Dependant.Value;
                                }
                            }

                            lastPartNotePart.PostfixWord = lastPartNotePart.PostfixWord.Preppend(dependencyLoop.Governor.Value);
                        }
                        else if (POSConstants.VerbLikePOS.Contains(pos))
                        {
                            noteLoop = new Note();

                            mainPartNotePart.MainWord = dependencyLoop.Governor.Value;

                            NotenizerDependency dobj = sentence.GetDependencyByShortName(dependencyLoop, Comparison.GovernorToGovernor, GrammaticalConstants.DirectObject);

                            if (dobj != null)
                            {
                                lastPartNotePart.MainWord = dobj.Dependant.Value;

                                NotenizerDependency neg = sentence.GetDependencyByShortName(dobj, Comparison.DependantToGovernor, GrammaticalConstants.NegationModifier);

                                if (neg != null)
                                    lastPartNotePart.PrefixWord = neg.Dependant.Value;
                            }

                            NotenizerDependency aux = sentence.GetDependencyByShortName(
                                dependencyLoop,
                                Comparison.GovernorToGovernor,
                                GrammaticalConstants.AuxModifier,
                                GrammaticalConstants.AuxModifierPassive);

                            if (aux != null)
                                mainPartNotePart.PrefixWord = aux.Dependant.Value;

                            // <== NMODS ==>
                            List<NotenizerDependency> nmodsList = sentence.GetDependenciesByShortName(
                                dependencyLoop,
                                Comparison.GovernorToGovernor,
                                GrammaticalConstants.NominalModifier);

                            if (nmodsList != null && nmodsList.Count > 0)
                            {
                                NotenizerDependency neg = sentence.GetDependencyByShortName(nmodsList.First(), Comparison.DependantToGovernor, GrammaticalConstants.NegationModifier);

                                if (neg == null)
                                {
                                    lastPartNotePart.PostfixWord =
                                        nmodsList.First().Relation.AdjustedSpecific + NotenizerConstants.WordDelimeter + nmodsList.First().Dependant.Value;
                                }
                                else
                                {
                                    lastPartNotePart.PostfixWord =
                                        nmodsList.First().Relation.AdjustedSpecific + NotenizerConstants.WordDelimeter +
                                        neg.Dependant.Value + NotenizerConstants.WordDelimeter + nmodsList.First().Dependant.Value;
                                }

                                // second nmod depending on first one
                                NotenizerDependency nmodSecond = sentence.GetDependencyByShortName(nmodsList.First(), Comparison.DependantToGovernor, GrammaticalConstants.NominalModifier);

                                if (nmodSecond != null)
                                {
                                    neg = sentence.GetDependencyByShortName(nmodsList.First(), Comparison.GovernorToGovernor, GrammaticalConstants.NegationModifier);

                                    if (neg == null)
                                    {
                                        lastPartNotePart.PostfixWord += NotenizerConstants.WordDelimeter +
                                            nmodSecond.Relation.AdjustedSpecific + NotenizerConstants.WordDelimeter + nmodSecond.Dependant.Value;
                                    }
                                    else
                                    {
                                        lastPartNotePart.PostfixWord += NotenizerConstants.WordDelimeter +
                                            nmodSecond.Relation.AdjustedSpecific + NotenizerConstants.WordDelimeter +
                                            neg.Dependant.Value + NotenizerConstants.WordDelimeter + nmodSecond.Dependant.Value;
                                    }
                                }
                            }
                        }

                    }
                    // typedDependency contains functions dep() and gov() which return IndexedWord which has function like lemma(), new(), and so on..
                    // typedDependency alson ocntains reln() wich is a relation (like nsubjpass, ...)

                    note.Concat(firstPartNotePart, mainPartNotePart, lastPartNotePart);
                    firstPartNotePart = new NotePart();
                    mainPartNotePart = new NotePart();
                    lastPartNotePart = new NotePart();
                }

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
                if (poses.Contains(dependencyLoop.Dependant.POS))
                    filteredDependencies.Add(dependencyLoop);
            }

            return filteredDependencies;
        }
    }
}
