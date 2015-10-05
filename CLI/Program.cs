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
			String text = @"Slovakia (Slovak: Slovensko) (Official name The Slovak Republic, Slovenská republika) is a country with no access to the ocean in Central Europe. It is bordered by Austria in the southwest, Hungary in the south, Ukraine in the east, Poland in the north and Czech Republic in the northwest. Its capital city is Bratislava. Other main cities are Košice, Banská Bystrica, Žilina, Trenčín, Nitra, Prešov, and Trnava. Slovakia is a member of the European Union.";
			//String text = "Clinton defeated Dote.";
			//String text = "Regan has died.";
			notenizer.RunCoreNLP(text);
			Console.ReadKey();
		}
	}
}
