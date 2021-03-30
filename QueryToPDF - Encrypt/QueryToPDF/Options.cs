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
        [Option('s', HelpText = "Server name destination")]
        public string server { get; set; }

        [Option('d', HelpText = "Connection string to database")]
        public string db { get; set; }

        [Option('q', HelpText = "Path query file to execute")]
        public string query { get; set; }

        [Option('o', HelpText = "Path output file")]
        public string output { get; set; }

        [Option('x', HelpText = "Path Template for html/xlst to PDF")]
        public string xsltfile { get; set; }

        [Option('v', HelpText = "for temporary variable")]
        public IEnumerable<string> variable { get; set; }

        [Option('c', HelpText = "Custom for PDF")]
        public IEnumerable<string> custom { get; set; }

        [Option('g', HelpText = "Custom page height PDF", Default = 0)]
        public float pageheight { get; set; }

        [Option('w', HelpText = "Custom page width PDF", Default = 0)]
        public float pagewidth { get; set; }

        [Option('t', HelpText = "Custom margin top PDF", Default = 0)]
        public float margintop { get; set; }

        [Option('b', HelpText = "Custom margin bottom PDF", Default = 0)]
        public float marginbottom { get; set; }

        [Option('m', HelpText = "Custom margin left PDF", Default = 0)]
        public float marginleft { get; set; }

        [Option('r', HelpText = "Custom margin right PDF", Default = 0)]
        public float marginright { get; set; }

        [Option('y', HelpText = "set owner pass pdf")]
        public string ownerpass { get; set; }

        [Option('p', HelpText = "set pass pdf")]
        public string pass { get; set; }

        [Option('l', HelpText = "set path logfile")]
        public string logfile { get; set; }

        [Option('z', HelpText = "log to screen")]
        public bool verbose { get; set; }


    }
}
