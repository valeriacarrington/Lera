using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Lera
{
    public class XMLAnalyzer
    {
        public IXMLAnalyzerStrategy strategy;

        public XMLAnalyzer(IXMLAnalyzerStrategy strategy)
        {
            this.strategy = strategy;
        }

        public List<string> SearchAttributes(XmlDocument xmlDocument)
        {
            return strategy.SearchAttributes(xmlDocument);
        }

        public List<XElement> SearchElementsByKeywords(XmlDocument xmlDocument, string keyword)
        {
            return strategy.SearchElementsByKeywords(xmlDocument, keyword);
        }
        public List<XElement> SearchElementsByAttribute(XmlDocument xmlDocument, string attributes)
        {
            return strategy.SearchElementsByAttribute(xmlDocument, attributes);
        }
    }

}
