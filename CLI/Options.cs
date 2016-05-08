using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsCLI
{
    /// <summary>
    /// Handles command line arguments.
    /// </summary>
    class Options
    {
        [Option('t', "text", Required = false, HelpText = "Text to process.", DefaultValue = "")]
        public string Text { get; set; }

        [Option('u', "url", Required = false, HelpText = "Url of wiki page containing article about country.", DefaultValue = "")]
        public string Url { get; set; }

        [Option("country", Required = false, HelpText = "Process article about country from wikipedia.", DefaultValue = "")]
        public string Country { get; set; }

        [Option('d', "db", Required = false, HelpText = "Name of database to connect to.", DefaultValue = null)]
        public string DatabaseName { get; set; }

        [Option('v', Required = false, HelpText = "Output debugging / help information.", DefaultValue = false)]
        public bool Verbose { get; set; }

        [Option('c', Required = false, HelpText = "Runs in console.", DefaultValue = false)]
        public bool IsConsole { get; set; }

        [Option("analyze", Required = false, HelpText = "Runs analysis on current database", DefaultValue = false)]
        public bool RunAnalysis { get; set; }
        /// <summary>
        /// Get's help command's text.
        /// </summary>
        /// <returns></returns>
        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
