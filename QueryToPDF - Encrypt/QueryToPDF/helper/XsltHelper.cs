using System;
using System.Xml.Xsl;
using System.Xml;
using System.Xml.Linq;

namespace QueryToPDF
{
    public class XsltHelper
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
