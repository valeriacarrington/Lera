using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Lera
{
    class LINQtoXMLStrategy : IXMLAnalyzerStrategy
    {
        public string GenerateDynamicQuery(string attribute, string keyword)
        {
            throw new NotImplementedException();
        }

        public List<string> SearchAttributes(XmlDocument xmlDocument)
        {
            XDocument xDoc = XDocument.Load(new XmlNodeReader(xmlDocument));
            List<string> attributes = new List<string>();

            if (xDoc != null)
            {
                foreach (XElement element in xDoc.Descendants())
                {
                    foreach (XAttribute attribute in element.Attributes())
                    {
                        string attributeName = attribute.Name.LocalName;
                        if (!attributes.Contains(attributeName))
                        {
                            attributes.Add(attributeName);
                        }
                    }
                }
            }

            return attributes;
        }

        public List<XElement> SearchElementsByKeywords(XmlDocument xmlDocument, string keyword)
        {
            XDocument xDoc = XDocument.Load(new XmlNodeReader(xmlDocument));
            List<XElement> resultElements = new List<XElement>();
            if (xDoc != null)
            {
                resultElements = xDoc.Descendants()
                    .Where(element =>
                        element.Parent != null &&
                        element.Parent == xDoc.Root &&
                        element.Value.Contains(keyword))
                    .ToList();
            }

            return resultElements;
        }
        public List<XElement> SearchElementsByAttribute(XmlDocument xmlDocument, string attribute)
        {
            XDocument xDoc = XDocument.Load(new XmlNodeReader(xmlDocument));
            List<XElement> resultElements = new List<XElement>();
            if (xDoc != null)
            {
                resultElements = xDoc.Descendants()
                    .Where(element =>
                        element.Attribute(attribute) != null)
                    .ToList();
            }

            return resultElements;
        }
    }
}
