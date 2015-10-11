using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using java.util;
using java.io;
using edu.stanford.nlp.trees;
using edu.stanford.nlp.ling;

namespace nsExtensions
{
    public static class Extensions
    {
		public static List<T> ToList<T>(this java.util.ArrayList javaArrayList)
		{
			List<T> list = new List<T>();

			foreach (var item in javaArrayList)
			{
				if (item.GetType() != typeof(T))
					throw new Exception("Collection containes object of other type than " + typeof(T));

				list.Add((T)item);
			}

			return list;
		}

		public static bool IsGrammaticalRelation(this GrammaticalRelation gr1, GrammaticalRelation gr2)
		{
			return gr1.getLongName() == gr2.getLongName();
		}

		public static String GetUniqueIdentifier(this IndexedWord idxWord)
		{
			return idxWord.word() 
				+ "/" 
				+ idxWord.get(typeof(CoreAnnotations.PartOfSpeechAnnotation)).ToString() 
				+ "/"
				+ idxWord.beginPosition() 
				+ "/"
				+ idxWord.endPosition();
		}

		public static String Preppend(this String source, String preppendingText)
		{
			return source.Preppend(preppendingText, true);
		}

		public static String Preppend(this String source, String preppendingText, bool bSeparate)
		{
			return source.Preppend(preppendingText, bSeparate ? " " : String.Empty);
		}

		public static String Preppend(this String source, String preppendingText, String separator)
		{
			return source = preppendingText + separator + source;
		}

		public static List<TypedDependency> FilterByPOS(this List<TypedDependency> dependencies, String[] poses)
		{
			List<TypedDependency> filteredDependencies = new List<TypedDependency>();

			foreach(TypedDependency dependencyLoop in dependencies)
			{
				if (poses.Contains(dependencyLoop.dep().get(typeof(CoreAnnotations.PartOfSpeechAnnotation)).ToString()))
					filteredDependencies.Add(dependencyLoop);
			}

			return filteredDependencies;
		}

		public static String GetAdjustedSpecific(this GrammaticalRelation relation)
		{
			return relation.getSpecific().AdjustSpecific();
		}

		public static String AdjustSpecific(this String specific)
		{
			if (specific == "agent")
				return "by";

			return specific;
		}
    }
}
