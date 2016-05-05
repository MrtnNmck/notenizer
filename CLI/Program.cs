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
        /// <summary>
        /// Main entry for program.
        /// </summary>
        /// <param name="args"></param>
        [STAThread]
        static void Main(string[] args)
		{
            Options options;
			Notenizer notenizer;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            options = new Options();
            notenizer = new Notenizer();

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
