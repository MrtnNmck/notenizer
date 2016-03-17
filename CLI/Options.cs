using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsCLI
{
    class Options
    {
        [Option("gui", Required = false, HelpText = "Run application with GUI.", DefaultValue = false)]
        public bool GUI { get; set; }

        [Option('t', "text", Required = false, HelpText = "Text to process.", DefaultValue = "")]
        public string Text { get; set; }

        public string GetUsage()
        {
            return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
