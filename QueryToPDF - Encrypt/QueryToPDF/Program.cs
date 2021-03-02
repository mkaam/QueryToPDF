using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Xsl;
using System.IO;
using System.Security.AccessControl;
using System.Xml;
using System.Xml.Linq;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Threading;
using System.ComponentModel.Design;
using NReco.PdfGenerator;
using iTextSharp.text.pdf;

namespace QueryToPDF
{
    class Program
    {
        static void Main(string[] args)
        {
            //args[0] -server
            //-- args[1] 1.server name: PMIIDSUBDEV42\DEV2012
            //args[2] -db
            //-- args[3] 2.DBName : Users
            //args[4] -query
            //-- args[5] 3.query file: \\PMIIDSUBDEV38\SSISFiles$\SummarySalesman\Summary.sql
            //args[6] -output
            //-- args[7] 4.output - full path pdf : \\PMIIDSUBDEV38\iSMSMirrorDirectory$\ftp_XXX_prd\3.PRD\Import\Import - PDF - InvoiceSummaryPerSalesman\20200729.PDF
            //args[8] -htmlfile
            //-- args[9] 5.xslt (HTML Formatting File): full path html file \\PMIIDSUBDEV38\iSMSMirrorDirectory$\ftp_XXX_prd\3.PRD\Import\Import - PDF - InvoiceSummaryPerSalesman\template.html
            // ex : QueryToPdf.exe -server PMIIDSUBDEV42\DEV2012 -db Users -query \\PMIIDSUBDEV38\SSISFiles$\SummarySalesman\Summary.sql
            // -output "\\PMIIDSUBDEV38\iSMSMirrorDirectory$\ftp_XXX_prd\3.PRD\Import\Import - PDF - InvoiceSummaryPerSalesman\20200729.PDF"
            // -htmlfile "\\PMIIDSUBDEV38\iSMSMirrorDirectory$\ftp_XXX_prd\3.PRD\Import\Import - PDF - InvoiceSummaryPerSalesman\template.html"

            string serverName = args[1];
            string dbName = args[3];
            string queryfile = args[5];
            string outputfile = args[7];

            // 2021-02-19 skip if output file is exist

            

            string pass = "";
            string ownerPass = "";
            float margintop = 0;
            float marginbtm = 0;
            float marginleft = 0;
            float marginright = 0;


            bool custom = false;
            float pageHeight =0, pageWidth=0;

            List<string> listOfParams = new List<string>();
            List<string> valueOfParams = new List<string>();

            string HTMLFormatFile = "";

            string di = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            string HTMLFolder = Path.GetDirectoryName(outputfile) + @"\";
            string fileName = Path.GetFileName(outputfile);
            string logPath = di + @"\Log\";
            string tempFolder = di + @"\Temp\";
       
            if (Directory.Exists(@logPath))
                {
                    Debug.WriteLine("Log Folder exists, please check if there is any errors");
                }
            else
                {
                    Directory.CreateDirectory(logPath);
                }

            if (Directory.Exists(@tempFolder))
            {
                Debug.WriteLine("Temp Folder exists, file will be saved here");
            }
            else
            {
                Directory.CreateDirectory(@tempFolder);
            }

            IEnumerable<FileInfo> oldfiles = GetOldFiles(tempFolder, DateTime.Now.AddDays(-3));
            foreach (FileInfo oldfile in oldfiles)
            {
                try
                {
                    File.Delete(oldfile.FullName);
                }
                catch (IOException) { }
            }

            IEnumerable<FileInfo> OldLogFiles = GetOldFiles(logPath, DateTime.Now.AddDays(-7));
            foreach (FileInfo oldfile in OldLogFiles)
            {
                try
                {
                    File.Delete(oldfile.FullName);
                }
                catch (IOException) {
                }
            }

            string logFile = logPath + Path.GetFileNameWithoutExtension(outputfile).ToString() + "_LOG.txt";

            if (File.Exists(outputfile))
            {
                CreateLog($"{outputfile} is exist. Skip...", logFile);
                Environment.Exit(0);
            }

            Debug.WriteLine("Log will be saved on " + logFile);

            if (Directory.Exists(@HTMLFolder))
            {
                Debug.WriteLine("HTML Folder exist");
            }
            else
            {
                Directory.CreateDirectory(@HTMLFolder);
            }

            //Console.WriteLine("WORKING ON" + di.ToString() + "\n" + "HTML WILL BE SAVED ON " + HTMLFolder);

            if (args.Length > 7)
            {
                for (int i = 8; i < args.Length; i++)
                {
                    if (args[i].Contains(".xslt"))
                    {
                        HTMLFormatFile = args[i];
                        if (File.Exists(HTMLFormatFile))
                        {
                            HTMLFormatFile = args[i];
                            Debug.WriteLine("PDF WILL BE FORMATTED AS " + HTMLFormatFile);
                        }
                        else
                        {
                            HTMLFormatFile = di + @"\raw-test.xslt";
                            CreateLog(".xslt file doesn't exist. please recheck!!! \n PDF WILL BE FORMATTED AS " + HTMLFormatFile, logFile);
                            //Console.WriteLine("PDF WILL BE FORMATTED AS " + HTMLFormatFile);
                        }
                    }
                    if (args[i].Contains('~'))
                    {
                        int indexOfSpecialChar = args[i].IndexOf('~');
                        if (indexOfSpecialChar != 1)
                        {
                            args[i] = args[i].Remove(0, indexOfSpecialChar + 1);
                        }

                        if (args[i].Contains('='))
                        {
                            int indexOfEqualsChar = args[i].IndexOf('=');
                            if (indexOfEqualsChar != -1)
                            {
                                valueOfParams.Add(args[i].Remove(0, indexOfEqualsChar + 1));
                                listOfParams.Add(args[i].Remove(indexOfEqualsChar, args[i].Length - indexOfEqualsChar));
                            }
                        }
                    }
                    if (args[i].Contains('='))
                    {
                        var VariableParam = args[i].Split('=');
                        int countParam=0;
                        foreach (string par in VariableParam)
                        {
                            countParam += 1;
                            if (countParam == 1) listOfParams.Add(par);
                            else if (countParam == VariableParam.Length) {
                                valueOfParams.Add(par);
                            }
                            else
                            {
                                if (par.Contains(' '))
                                {
                                    string tempstr = par.Split(' ')[par.Split(' ').GetUpperBound(0)];
                                    listOfParams.Add(tempstr);
                                    valueOfParams.Add(par.Replace($" {tempstr}", ""));
                                }
                                else
                                {
                                    valueOfParams.Add(par);
                                }
                            }
                            
                        }
                        if (args[i].Contains("pageHeight"))
                        {
                            int indexOfEqualsChar = args[i].IndexOf('=');
                            pageHeight = float.Parse(args[i].Remove(0, indexOfEqualsChar + 1), System.Globalization.CultureInfo.InvariantCulture);
                        }
                        if (args[i].Contains("pageWidth"))
                        {
                            int indexOfEqualsChar = args[i].IndexOf('=');
                            pageWidth = float.Parse(args[i].Remove(0, indexOfEqualsChar + 1), System.Globalization.CultureInfo.InvariantCulture);
                        }

                        // 20210125 remark by Aam : wrong logic to determine multiple variable, fixing on above this code
                        //int indexOfEqualsChar = args[i].IndexOf('=');
                        //if (indexOfEqualsChar != -1)
                        //{
                        //    valueOfParams.Add(args[i].Remove(0, indexOfEqualsChar + 1));
                        //    listOfParams.Add(args[i].Remove(indexOfEqualsChar, args[i].Length - indexOfEqualsChar));
                        //    //pageWidth = 76
                        //    if (args[i].Contains("pageHeight"))
                        //    {
                        //        pageHeight = float.Parse(args[i].Remove(0, indexOfEqualsChar + 1), System.Globalization.CultureInfo.InvariantCulture);
                        //    }
                        //    if (args[i].Contains("pageWidth"))
                        //    {
                        //        pageWidth = float.Parse(args[i].Remove(0, indexOfEqualsChar + 1), System.Globalization.CultureInfo.InvariantCulture);
                        //    }
                        //}
                    }

                    if(args[i] == "custom")
                    {
                        custom = true;
                    }

                    if (args[i].Contains("pass"))
                    {
                        int indexOfEqualsChar = args[i].IndexOf('=');
                        if (indexOfEqualsChar != -1)
                        {
                            pass = args[i].Remove(0, indexOfEqualsChar + 1);
                            var charsToRemove = new String[] { "'", "\"" };

                            foreach(var c in charsToRemove)
                            {
                                pass = pass.Replace(c, string.Empty);
                            }
                        }
                    }

                    if (args[i].Contains("ownerPass"))
                    {
                        int indexOfEqualsChar = args[i].IndexOf('=');
                        if (indexOfEqualsChar != -1)
                        {
                            ownerPass = args[i].Remove(0, indexOfEqualsChar + 1);
                            var charsToRemove = new String[] { "'", "\"" };

                            foreach (var c in charsToRemove)
                            {
                                ownerPass = ownerPass.Replace(c, string.Empty);
                            }
                        }
                    }

                    if (args[i].Contains("margintop"))
                    {
                        int indexOfEqualsChar = args[i].IndexOf('=');
                        if (indexOfEqualsChar != -1)
                        {
                            var charsToRemove = new String[] { "'", "\"" };
                            foreach (var c in charsToRemove)
                            {
                                args[i] = args[i].Replace(c, string.Empty);
                            }

                            margintop = float.Parse(args[i].Remove(0, indexOfEqualsChar + 1));
                        }
                    }

                    if (args[i].Contains("marginbtm"))
                    {
                        int indexOfEqualsChar = args[i].IndexOf('=');
                        if (indexOfEqualsChar != -1)
                        {
                            var charsToRemove = new String[] { "'", "\"" };
                            foreach (var c in charsToRemove)
                            {
                                args[i] = args[i].Replace(c, string.Empty);
                            }

                            marginbtm = float.Parse(args[i].Remove(0, indexOfEqualsChar + 1));
                        }
                    }

                    if (args[i].Contains("marginleft"))
                    {
                        int indexOfEqualsChar = args[i].IndexOf('=');
                        if (indexOfEqualsChar != -1)
                        {
                            var charsToRemove = new String[] { "'", "\"" };
                            foreach (var c in charsToRemove)
                            {
                                args[i] = args[i].Replace(c, string.Empty);
                            }

                            marginleft = float.Parse(args[i].Remove(0, indexOfEqualsChar + 1));
                        }
                    }

                    if (args[i].Contains("marginright"))
                    {
                        int indexOfEqualsChar = args[i].IndexOf('=');
                        if (indexOfEqualsChar != -1)
                        {
                            var charsToRemove = new String[] { "'", "\"" };
                            foreach (var c in charsToRemove)
                            {
                                args[i] = args[i].Replace(c, string.Empty);
                            }

                            marginright = float.Parse(args[i].Remove(0, indexOfEqualsChar + 1));
                        }
                    }
                }
            }
            else
            {
                //HTMLFormatFile = Path.GetFullPath(Path.Combine(di, @"\formatHTML\raw.xslt")).ToString();
                HTMLFormatFile = di + @"\raw.xslt";
                if (File.Exists(HTMLFormatFile))
                {
                    CreateLog("PDF WILL BE FORMATTED AS " + HTMLFormatFile, logFile);
                }
                else
                {
                    CreateLog(".xslt file doesn't exist. please recheck!!!", logFile);
                }
                //Console.WriteLine();
            }

            //if(File.Exists(HTMLFormatFile))
            //{
            //}

            // -----
            SqlConnection sqlcon = new SqlConnection("server=" + @serverName + ";" +
                                       "Trusted_Connection=yes;" +
                                       "database=" + dbName + "; " +
                                       "connection timeout=30");
            try
            {
                if (sqlcon.State == ConnectionState.Closed)
                    sqlcon.Open();
                //Console.WriteLine("Open Connection Succeess");
            }
            catch (Exception e)
            {
                CreateLog(e.Message, logFile);
            }

            string scriptSQL = File.ReadAllText(@queryfile).ToString();

            String[] valueOfParamsArr = valueOfParams.ToArray();
            String[] listOfParamsArr = listOfParams.ToArray();

            if (args.Length > 7)
            {
                for (int i = 0; i < listOfParamsArr.Length; i++)
                {
                    if (scriptSQL.Contains(listOfParamsArr[i]) == true)
                    {
                        scriptSQL = scriptSQL.Replace(listOfParamsArr[i], valueOfParamsArr[i]);
                    }
                }
            }

            //Console.WriteLine(scriptSQL);
            SqlDataAdapter daAuthors;
            DataSet dsPubs;
            DataTable dt = new DataTable();
            try
            {
                daAuthors = new SqlDataAdapter(scriptSQL, sqlcon);
                dsPubs = new DataSet("Pubs");

                // 2021-01-26 remark by Aam, if column name not standard , this cause trouble, eg: [nama customer]
                //daAuthors.FillSchema(dsPubs, SchemaType.Source, "Authors");
                daAuthors.Fill(dsPubs, "Authors");
               
                dt = dsPubs.Tables["Authors"];
            }
            catch (Exception ex)
            {
                CreateLog(ex.Message, logFile);
            }


            if (dt.Rows.Count > 0)
            {
                TextWriter writer = new StringWriter();
                dt.WriteXml(writer);
                String result;
                StreamWriter sw;
                string tempHTMLFile = @tempFolder + Path.GetFileNameWithoutExtension(outputfile).ToString() + ".html";

                //Console.WriteLine("Jumlah Data " + dt.Rows.Count);

                //string tempPDFFile = @tempFolder + fileName;

                //if (File.Exists(tempHTMLFile))
                //{
                //    File.Delete(tempHTMLFile);
                //}

                //if (File.Exists(tempPDFFile))
                //{
                //    File.Delete(tempPDFFile);
                //}

                //if (File.Exists(@outputfile))
                //{
                //    File.Delete(@outputfile);
                //}

                try
                {
                    result = new XsltHelper().GetValue(@HTMLFormatFile, writer.ToString());
                    sw = new StreamWriter(tempHTMLFile, false);
                    sw.WriteLine(result);
                    sw.Flush();
                    sw.Close();
                }
                catch (Exception e)
                {
                    CreateLog(e.Message, logFile);
                }

                //Console.WriteLine("HTML SAVED ON " + tempHTMLFile);

                HtmlToPdfConverter htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();
                try
                {
                    if (custom == true)
                    {
                        htmlToPdf.CustomWkHtmlArgs = " --print-media-type --dpi 300 --disable-smart-shrinking";
                        //htmlToPdf.Zoom = 0.85f;
                        htmlToPdf.PageHeight = pageHeight;
                        htmlToPdf.PageWidth = pageWidth;
                    }
                    var margins = new PageMargins
                    {
                        Top = margintop,
                        Bottom = marginbtm,
                        Left = marginleft,
                        Right = marginright
                    };

                    htmlToPdf.Margins = margins;

                    string temp_outputfile = @tempFolder + "temp_" + fileName;
                    htmlToPdf.GeneratePdfFromFile(tempHTMLFile, null, temp_outputfile);

                    using (var input = new FileStream(temp_outputfile, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        using (var output = new FileStream(outputfile, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            if (ownerPass == "") ownerPass = pass;
                            var reader = new PdfReader(input);
                            PdfEncryptor.Encrypt(reader, output, true, pass, ownerPass, PdfWriter.ALLOW_PRINTING);
                        }
                    }

                    //File.Copy(@tempFolder + fileName, outputfile);
                    //File.Delete(temp_outputfile);
                    //File.Delete(tempHTMLFile);
                }
                catch (Exception e)
                {
                    CreateLog("Generate PDF Error Cause of " + e.Message, logFile);
                }
            }



    }

        public static void CreateLog(string errorMessage, string logFile)
        {
            if (File.Exists(logFile))
            {
                using (StreamWriter w = File.AppendText(logFile))
                {
                    Log(errorMessage, w);
                }

                using (StreamReader r = File.OpenText(logFile))
                {
                    DumpLog(r);
                }
            }
            else
            {
                using (StreamWriter w = File.CreateText(logFile))
                {
                    Log(errorMessage, w);
                }

                using (StreamReader r = File.OpenText(logFile))
                {
                    DumpLog(r);
                }
            }
            Console.WriteLine("Application Error. Cause of : " + errorMessage);
            //Console.ReadLine();
            //Environment.Exit(0);
        }
        public static void Log(string logMessage, TextWriter w)
        {
            w.Write("\r\nLog Entry : ");
            w.WriteLine($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}");
            w.WriteLine("  :");
            w.WriteLine($"  :{logMessage}");
            w.WriteLine("-------------------------------");
        }

        public static void DumpLog(StreamReader r)
        {
            string line;
            while ((line = r.ReadLine()) != null)
            {
                Console.WriteLine(line);
            }
        }

        public static IEnumerable<System.IO.FileInfo> GetOldFiles(string path,
                    DateTime LastDay)
        {
            if (LastDay == null)
                LastDay = DateTime.Now.AddDays(-7);

            var oldfiles = new DirectoryInfo(path)
                                     .EnumerateFiles()
                                     .Select(x => {
                                         x.Refresh();
                                         return x;
                                     })
                                     .Where(x => x.CreationTime.Date < LastDay || x.LastWriteTime < LastDay);

            return oldfiles;
        }
    }
    class XsltHelper
    {
        public string GetValue(string templatePath, string xmlString)
        {
            XDocument xmlObj = XDocument.Parse(xmlString);
            XDocument result = GetResultXml(templatePath, xmlObj);

            if (result == null)
            {
                return String.Empty;
            }
            else
            {
                return result.Document.ToString();
            }
        }

        private XDocument GetResultXml(string templatePath, XDocument xmlObj)
        {
            XDocument result = new XDocument();
            using (XmlWriter writer = result.CreateWriter())
            {
                XslCompiledTransform xslt = new XslCompiledTransform();
                xslt.Load(templatePath);
                xslt.Transform(xmlObj.CreateReader(), writer);
            }
            return result;
        }
    }
}
