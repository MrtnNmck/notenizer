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

namespace nsNotenizer
{
    public class Notenizer
    {
		public Notenizer()
		{

		}

		public void RunCoreNLP(String text = null)
		{
			// Path to the folder with models extracted from `stanford-corenlp-3.5.2-models.jar`
			//var jarRoot = @"C:\Programs\StanfordNLP\models\stanford-corenlp-full-2015-04-20";
			var jarRoot = @"stanford-corenlp-3.5.2-models";

			// Text for processing
			if (text == null)
				text = "Kosgi Santosh sent an email to Stanford University. He didn't get a reply.";

			// Annotation pipeline configuration
			var props = new Properties();
			props.setProperty("annotators", "tokenize, ssplit, pos, lemma, ner, parse, dcoref");
			//props.setProperty("annotators", "tokenize, ssplit, parse");
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


			String firstPart = String.Empty;
			String midPart = String.Empty;
			String endPart = String.Empty;
			String note = String.Empty;
			Dictionary<String, String> sentencesNoted = new Dictionary<String, String>();
			bool findCOP = false;

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

					if (typedDependency.reln().IsGrammaticalRelation(EnglishGrammaticalRelations.NOMINAL_SUBJECT)
						|| typedDependency.reln().IsGrammaticalRelation(EnglishGrammaticalRelations.NOMINAL_PASSIVE_SUBJECT))
					{
						firstPart = typedDependency.dep().word();

						String pos = typedDependency.gov().get(typeof(CoreAnnotations.PartOfSpeechAnnotation)).ToString();
						if (pos == "NN" || pos == "NNP")
						{
							TypedDependency compound = list
								.Where(x => x.reln().getLongName() == "compound modifier")
								.Where(y => y.gov().toString() == typedDependency.dep().toString())
								.Select(z => z).FirstOrDefault();

							firstPart = compound != null ? compound.dep().word() + " " + firstPart : firstPart;

							TypedDependency cop = list
								.Where(x => x.reln().IsGrammaticalRelation(EnglishGrammaticalRelations.COPULA))
								.Where(y => y.gov().toString() == typedDependency.gov().toString())
								.Select(z => z).FirstOrDefault();

							midPart = cop.dep().word();
							endPart = typedDependency.gov().word();
						}
						else if (pos == "VB" || pos == "VBN" || pos == "VBG" || pos == "VBD" || pos == "VBP" || pos == "VBZ")
						{
							midPart = typedDependency.gov().word();

							TypedDependency dobj = list
								.Where(x => x.reln().IsGrammaticalRelation(EnglishGrammaticalRelations.DIRECT_OBJECT))
								.Where(y => y.gov().toString() == typedDependency.gov().toString())
								.Select(z => z).FirstOrDefault();

							TypedDependency aux = list
								.Where(x => x.reln().IsGrammaticalRelation(EnglishGrammaticalRelations.AUX_MODIFIER) || x.reln().IsGrammaticalRelation(EnglishGrammaticalRelations.AUX_PASSIVE_MODIFIER))
								.Where(y => y.gov().toString() == typedDependency.gov().toString())
								.Select(z => z).FirstOrDefault();

							if (aux != null)
								endPart += aux.dep().word();

							if (dobj != null)
								endPart += dobj.dep().word();
						}

						sentencesNoted.Add(sentence.toString(), firstPart + " " + midPart + " " + endPart);

						firstPart = midPart = endPart = String.Empty;
					}
					// typedDependency contains functions dep() and gov() which return IndexedWord which has function like lemma(), new(), and so on..
					// typedDependency alson ocntains reln() wich is a relation (like nsubjpass, ...)
				}
			}

			Console.WriteLine();
			Console.WriteLine("<======== NOTES ========>");

			foreach (KeyValuePair<String, String> noteLoop in sentencesNoted)
			{
				Console.WriteLine(noteLoop.Key + " --> " + noteLoop.Value);
			}

		}
    }
}
