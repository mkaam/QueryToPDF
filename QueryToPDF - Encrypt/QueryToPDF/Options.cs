using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace QueryToPDF
{
    class Options
    {
        [Option(HelpText = "Server name destination")]
        public string server { get; set; }
        [Option(HelpText = "Connection string to database")]
        public string db { get; set; }
        [Option(HelpText = "Path query file to execute")]
        public string query { get; set; }
        [Option(HelpText = "Path output file")]
        public string output { get; set; }
        [Option(HelpText = "Path Template for html/xlst to PDF")]
        public string xsltfile { get; set; }
        [Option(HelpText = "for temporary variable")]
        public IEnumerable<string> variable { get; set; }
        [Option(HelpText = "Custom for PDF")]
        public IEnumerable<string> custom { get; set; }

        [Option(HelpText = "Custom page height PDF", Default = 0)]
        public float pageheight { get; set; }
        [Option(HelpText = "Custom page width PDF", Default = 0)]
        public float pagewidth { get; set; }
        [Option(HelpText = "Custom margin top PDF", Default = 0)]
        public float margintop { get; set; }
        [Option(HelpText = "Custom margin bottom PDF", Default = 0)]
        public float marginbottom { get; set; }
        [Option(HelpText = "Custom margin left PDF", Default = 0)]
        public float marginleft { get; set; }
        [Option(HelpText = "Custom margin right PDF", Default = 0)]
        public float marginright { get; set; }
        [Option(HelpText = "set owner pass pdf")]
        public string ownerpass { get; set; }
        [Option(HelpText = "set pass pdf")]
        public string pass { get; set; }
        [Option(HelpText = "set path logfile")]
        public string logfile { get; set; }
        public bool verbose { get; set; }


    }
}
