using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Lera
{
    public interface IXMLAnalyzerStrategy
    {
        List<string> SearchAttributes(XmlDocument xmlDocument);
        List<XElement> SearchElementsByKeywords(XmlDocument xmlDocument, string keyword);
        List<XElement> SearchElementsByAttribute(XmlDocument xmlDocument, string attribute);
    }

}
