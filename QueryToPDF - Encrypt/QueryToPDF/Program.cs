using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;
using NReco.PdfGenerator;
using iTextSharp.text.pdf;
using CommandLine;
using NLog.Layouts;
using NLog;

namespace QueryToPDF
{
    class Program
    {
        private static bool ParseError = false;
        private static string RootPath;
        private static string Exepath;
        private static Stopwatch _watch;
        private static Logger logger;
        private static string LogPath;
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                logger = new Logger("log");
                Process(args);                
            }
            else
            {
                string[] argManual = Console.ReadLine().Split(' ');
                Main(argManual);
            }
        }

        #region new code

        public static void Process(string[] args)
        {
            try
            {
                var argument = args.Select(x => x.Contains("--") ? x : (x.Contains("-") ? ($"\"{x}\"") : x)).ToArray();
                string strArg = string.Join(" ", args);
                Exepath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                RootPath = Exepath;
                LogPath = Path.Combine(RootPath, "Logs");
                logger.Debug("Application Start");
                _watch = new Stopwatch();
                _watch.Start();
                var parser = new Parser(config =>
                {
                    config.IgnoreUnknownArguments = false;
                    config.CaseSensitive = false;
                    config.AutoHelp = true;
                    config.AutoVersion = true;
                    config.HelpWriter = Console.Error;
                });
                var result = parser.ParseArguments<Options>(args)
                    .WithParsed<Options>(s => RunOptions(s))
                    .WithNotParsed(errors => HandleParseError(errors));

                if (!ParseError)
                {
                    logger.Debug($"{strArg}");
                    _watch.Stop();
                    logger.Debug($"Application Finished. Elapsed time: {_watch.ElapsedMilliseconds}ms");
                }
            }catch(Exception ex )
            {
                logger.Error($"Error : {ex.Message}");
            }
        }

        public static void HandleParseError(IEnumerable<Error> errs)
        {
            ParseError = true;

            if (errs.Any(x => x is HelpRequestedError || x is VersionRequestedError))
            {
            }
            else
                Console.WriteLine("Parameter unknown, please check the documentation or use '--help' for more information");


        }

        public static void RunOptions(Options opt)
        {
            PathConfigure(opt);
            LoggerConfigure(opt);
            if (!string.IsNullOrEmpty(opt.query))
            {
                var queryString = "";
                using (StreamReader sr = new StreamReader(opt.query))
                {
                    queryString = sr.ReadToEnd();
                }

                if (opt.variable.Count() > 0)
                {
                    foreach (var x in opt.variable)
                    {
                        var dataParam = x.Split('=');
                        queryString = queryString.Replace(dataParam[0], dataParam[1]);
                    }
                }

                DataTable dt = new DataTable();
                var connStr = $"Data Source={opt.server};Initial Catalog={opt.db};Integrated Security=True;Connection Timeout=60;";
                using (SqlConnection sqlConn = new SqlConnection(connStr))
                {
                    sqlConn.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = queryString;
                        cmd.CommandTimeout = 3600;
                        cmd.Connection = sqlConn;

                        using (SqlDataAdapter da = new SqlDataAdapter())
                        {
                            using (DataSet ds = new DataSet())
                            {
                                da.SelectCommand = cmd;
                                da.Fill(ds);
                                dt = ds.Tables[0];
                            }
                        }
                    }
                }
                if (dt.Rows.Count > 0)
                    CreatePDF(dt, opt);
            }
        }

        public static void CreatePDF(DataTable dt, Options opt)
        {
            TextWriter writer = new StringWriter();
            dt.WriteXml(writer);
            string result;
            StreamWriter streamWriter;
            string di = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            string fileName = Path.GetFileName(opt.output);
            string tempFolder = di + @"\Temp\";

            string templateHTML = $"tempFolder {Path.GetFileNameWithoutExtension(fileName)}.html";
            string htmlFile = opt.xsltfile;
            try
            {
                result = new XsltHelper().GetValue(htmlFile, writer.ToString());
                streamWriter = new StreamWriter(templateHTML, false);
                streamWriter.WriteLine(result);
                streamWriter.Flush();
                streamWriter.Close();
            }
            catch (Exception e)
            {
                logger.Error($"Error : {e.Message}");
                //CreateLog(e.Message, logFile);
            }

            HtmlToPdfConverter htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();
            try
            {

                if (opt.pagewidth != 0)
                    htmlToPdf.PageWidth = opt.pagewidth;

                if (opt.pageheight != 0)
                    htmlToPdf.PageHeight = opt.pageheight;

                if(opt.pagewidth != 0 || opt.pageheight != 0)
                    htmlToPdf.CustomWkHtmlArgs = " --print-media-type --dpi 300 --disable-smart-shrinking";


                var margins = new PageMargins
                {
                    Top = opt.margintop,
                    Bottom = opt.marginbottom,
                    Left = opt.marginleft,
                    Right = opt.marginright
                };

                htmlToPdf.Margins = margins;

                string temp_outputfile = @tempFolder + "temp_" + fileName;
                htmlToPdf.GeneratePdfFromFile(templateHTML, null, temp_outputfile);

                using (var input = new FileStream(temp_outputfile, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var pathBD = opt.output.Split('\\');

                    var fileNamestr = pathBD[pathBD.Length - 1];

                    var outpuDest = opt.output.Replace(fileNamestr, "");

                    if (!Directory.Exists(outpuDest))
                    {
                        Directory.CreateDirectory(outpuDest);
                    }

                    using (var output = new FileStream(opt.output, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        if (opt.ownerpass == "") opt.ownerpass = opt.pass;
                        var reader = new PdfReader(input);
                        PdfEncryptor.Encrypt(reader, output, true, opt.pass, opt.ownerpass, PdfWriter.ALLOW_PRINTING);
                    }
                }
            }
            catch (Exception e)
            {
                logger.Error($"Generate PDF Error Cause of {e.Message}");
            }

        }
        public static void PathConfigure(Options opts)
        {
            //// Logs
            //if (opts.LogFile != null && Path.GetFileName(opts.LogFile) == opts.LogFile)
            //{
            //    if (!Directory.Exists(LogPath))
            //        Directory.CreateDirectory(LogPath);

            //    opts.LogFile = $"{Path.Combine(LogPath, opts.LogFile)}";
            //}

            //
            if (opts.output != null && Path.GetFileName(opts.output) == opts.output)
            {
                opts.output = $"{Path.Combine(RootPath, opts.output)}";
            }

            if (opts.xsltfile != null && Path.GetFileName(opts.xsltfile) == opts.xsltfile)
            {
                opts.xsltfile = $"{Path.Combine(RootPath, opts.xsltfile)}";
            }
            if (opts.query != null && Path.GetFileName(opts.query) == opts.query)
            {
                opts.query = $"{Path.Combine(RootPath, opts.query)}";
            }

        }
        public static void LoggerConfigure(Options opts)
        {
            var config = new NLog.Config.LoggingConfiguration();

            // Targets where to log to: File and Console
            var logfile = new NLog.Targets.FileTarget("logfile");
            if (opts.logfile != null)
            {
                if (Path.GetFileName(opts.logfile) == opts.logfile)
                    logfile.FileName = $"{Path.Combine(Path.Combine(RootPath, "Logs"), opts.logfile)}";
                else
                    logfile.FileName = $"{opts.logfile}";
            }
            else
                logfile.FileName = $"{Path.Combine(Path.Combine(RootPath, "Logs"), $"{DateTime.Now.ToString("yyyyMMdd")}.csv")}";

            logfile.MaxArchiveFiles = 60;
            logfile.ArchiveAboveSize = 10240000;

            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");
            if (opts.verbose)
                config.AddRule(LogLevel.Trace, LogLevel.Fatal, logconsole);
            else
                config.AddRule(LogLevel.Error, LogLevel.Fatal, logconsole);

            config.AddRule(LogLevel.Trace, LogLevel.Fatal, logfile);

            // design layout for file log rotation
            CsvLayout layout = new CsvLayout();
            layout.Delimiter = CsvColumnDelimiterMode.Comma;
            layout.Quoting = CsvQuotingMode.Auto;
            layout.Columns.Add(new CsvColumn("Start Time", "${longdate}"));
            layout.Columns.Add(new CsvColumn("Elapsed Time", "${elapsed-time}"));
            layout.Columns.Add(new CsvColumn("Machine Name", "${machinename}"));
            layout.Columns.Add(new CsvColumn("Login", "${windows-identity}"));
            layout.Columns.Add(new CsvColumn("Level", "${uppercase:${level}}"));
            layout.Columns.Add(new CsvColumn("Message", "${message}"));
            layout.Columns.Add(new CsvColumn("Exception", "${exception:format=toString}"));
            logfile.Layout = layout;

            // design layout for console log rotation
            SimpleLayout ConsoleLayout = new SimpleLayout("${longdate}:${message}\n${exception}");
            logconsole.Layout = ConsoleLayout;

            // Apply config           
            NLog.LogManager.Configuration = config;
        }
        #endregion
    }

}
