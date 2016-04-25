using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nsNotenizer;
using nsDB;
using System.Windows.Forms;
using nsGUI;
using nsServices.WebServices;

namespace nsCLI
{
	class Program
	{
        [STAThread]
        static void Main(string[] args)
		{
            Options options = new Options();
			Notenizer notenizer = new Notenizer();
            //String text = @"Slovakia (Slovak: Slovensko) (Official name The Slovak Republic, Slovenská republika) is a country with no access to the ocean in Central Europe. It is bordered by Austria in the southwest, Hungary in the south, Ukraine in the east, Poland in the north and Czech Republic in the northwest. Its capital city is Bratislava. Other main cities are Košice, Banská Bystrica, Žilina, Trenčín, Nitra, Prešov, and Trnava. Slovakia is a member of the European Union.";
            //String text = "Hungary is a country in Central Europe. Its capital city is Budapest. Hungary is slightly bigger than its western neighbour Austria and has about 10 million inhabitants. Other countries that border Hungary are Slovakia, Ukraine, Romania, Serbia, Croatia and Slovenia. Hungary's official language is the Hungarian language. It has been a member of the European Union (EU) since 2004. In Hungarian the country is called Magyarország (Hungary) or Magyar Köztársaság (Hungarian Republic). This is named after the Magyar tribes who came to Hungary in the late 9th century.";
            //String text = "Czech Republic (Czech: Česká republika) is a country in Central Europe, sometimes also known as Czechia (Czech: Česko). The capital and the biggest city is Prague. The currency is the Czech Crown (koruna česká - CZK). 1€ is about 27 CZK. The president of the Czech Republic is Miloš Zeman. The Czech Republic's population is about 10.5 million. The local language is Czech language. The Czech language is a Slavic language. It is related to languages like Slovak and Polish. In 1993 the Czech Ministry of Foreign Affairs announced that the name Czechia be used for the country outside of formal official documents. This has not caught on in English usage. Czech Republic has no sea; its neighbour countries are Germany, Austria, Slovakia, and Poland.";
            //String text = "Clinton defeated Dote.";
            //String text = "Regan has died.";
            //String text = "She looks very beautiful.";

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                if (options.DatabaseName != null)
                    ConnectionManager.DatabaseName = options.DatabaseName;

                if (options.GUI)
                {
                    if (options.Text != String.Empty)
                        Application.Run(new FormMain(options.Text));
                    else if (options.Url != String.Empty)
                        Application.Run(new FormMain(WikiParser.Parse(options.Url)));
                    else if (options.Country != String.Empty)
                        Application.Run(new FormMain(WikiParser.ParseCountry(options.Country)));
                    else
                        Application.Run(new FormMain());
                }
                else if (!options.GUI)
                {
                    if (options.Text != String.Empty)
                        notenizer.RunCoreNLP(options.Text);
                    else if (options.Url != String.Empty)
                        Application.Run(new FormMain(WikiParser.Parse(options.Url)));
                    else if (options.Country != String.Empty)
                        Application.Run(new FormMain(WikiParser.ParseCountry(options.Country)));
                }
            }
        }
	}
}
