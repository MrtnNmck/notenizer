using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nsNotenizer;

namespace CLI
{
	class Program
	{
		static void Main(string[] args)
		{
			Notenizer notenizer = new Notenizer();
			notenizer.RunCoreNLP();
			Console.ReadKey();
		}
	}
}
