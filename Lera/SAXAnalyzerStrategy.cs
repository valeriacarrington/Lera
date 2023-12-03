using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using Microsoft.Maui.Controls;

namespace Lera
{
    public class SAXAnalyzerStrategy : IXMLAnalyzerStrategy
    {
        public List<string> SearchAttributes(XmlDocument xmlDocument)
        {
            List<string> attributes = new List<string>();

            if (xmlDocument != null)
            {
                // Створюємо SAX парсер
                using (XmlReader reader = new XmlNodeReader(xmlDocument))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            // Отримуємо всі атрибути елементу
                            for (int i = 0; i < reader.AttributeCount; i++)
                            {
                                reader.MoveToAttribute(i);
                                string attributeName = reader.Name;

                                // Додати атрибут до списку, якщо його ще немає там
                                if (!attributes.Contains(attributeName))
                                {
                                    attributes.Add(attributeName);
                                }
                            }
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
                using (XmlReader reader = new XmlNodeReader(xmlDocument))
                {
                    XElement currentElement = null;

                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            currentElement = XElement.Load(reader.ReadSubtree());
                            foreach (var element in currentElement.Elements())
                                if (element.Value.Contains(keyword))
                            {
                                resultElements.Add(element);
                            }
                        }

                        
                    }
                }
            }

            return resultElements;
        }
        public List<XElement> SearchElementsByAttribute(XmlDocument xmlDocument, string attribute)
        {
            List<XElement> resultElements = new List<XElement>();

            if (xmlDocument != null)
            {
                using (XmlReader reader = new XmlNodeReader(xmlDocument))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            if (reader.HasAttributes)
                            {
                                // Перевіряємо наявність атрибута
                                string attrValue = reader.GetAttribute(attribute);
                                if (!string.IsNullOrEmpty(attrValue))
                                {
                                    XElement element = XElement.Load(reader.ReadSubtree());
                                    resultElements.Add(element);
                                }
                            }

                        }
                    }
                }
            }
            return resultElements;
        }

    }
}
