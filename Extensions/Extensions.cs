using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using java.util;
using java.io;
using edu.stanford.nlp.trees;

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
    }
}
