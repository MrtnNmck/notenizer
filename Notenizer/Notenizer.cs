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
			var sentences = annotation.get(typeof(CoreAnnotations.SentencesAnnotation));
			if (sentences == null)
			{
				return;
			}

			foreach (Annotation sentence in sentences as ArrayList)
			{
				Console.WriteLine(sentence);
			}
		}
    }
}
