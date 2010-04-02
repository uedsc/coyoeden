using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Cynthia.Web.Framework
{
    public static class XmlHelper
    {
        public static void AddNode(XmlDocument xmlDoc, string name, string content)
        {
            XmlElement elem = xmlDoc.CreateElement(name);
            XmlText text = xmlDoc.CreateTextNode(content);
            xmlDoc.DocumentElement.AppendChild(elem);
            xmlDoc.DocumentElement.LastChild.AppendChild(text);
           
        }
    }
}
