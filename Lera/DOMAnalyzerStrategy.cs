
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Lera
{
    class DOMAnalyzerStrategy : IXMLAnalyzerStrategy
    {
        public List<string> SearchAttributes(XmlDocument xmlDocument)
        {
            List<string> attributes = new List<string>();

            if (xmlDocument != null)
            {
                XmlNodeList elements = xmlDocument.GetElementsByTagName("*");

                foreach (XmlElement element in elements)
                {
                    foreach (XmlAttribute attribute in element.Attributes)
                    {
                        string attributeName = attribute.Name;
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
            List<XElement> resultElements = new List<XElement>();

            if (xmlDocument != null)
            {
                XmlNodeList elements = xmlDocument.GetElementsByTagName("*");

                foreach (XmlElement element in elements)
                {
                    // Перевіряємо, чи цей елемент є елементом другого рівня
                    if (element.ParentNode == xmlDocument.DocumentElement)
                    {
                            // Перевіряємо наявність атрибута та ключового слова
                            if (element.InnerText.Contains(keyword))
                            {
                                // Додаємо знайдений елемент в список результатів
                                resultElements.Add(XElement.Load(new XmlNodeReader(element)));
                            }
                        
                    }
                }
            }

            return resultElements;
        }
        public List<XElement> SearchElementsByAttribute(XmlDocument xmlDocument,string attribute)
        {
            List<XElement> resultElements = new List<XElement>();

            if (xmlDocument != null)
            {
                XmlNodeList elements = xmlDocument.GetElementsByTagName("*");

                foreach (XmlElement element in elements)
                {
                    foreach (XmlElement serviceElement in element.GetElementsByTagName("*"))
                    {
                        // Перевіряємо наявність атрибута та ключового слова
                        if (serviceElement.HasAttribute(attribute))
                        {
                            // Додаємо знайдений елемент в список результатів
                            resultElements.Add(XElement.Load(new XmlNodeReader(serviceElement)));
                        }
                    }
                }
            }

            return resultElements;
        }

    }
}
