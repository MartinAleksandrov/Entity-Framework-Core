using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace CarDealer.Utilities
{
    public class XmlHelper
    {
        public T Deserialize<T>(string root,string xmlInput)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute(root);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T),xmlRoot);

            using StringReader stringReader = new StringReader(xmlInput);

            var dtos = (T)xmlSerializer.Deserialize(stringReader)!;

            return dtos;
        }

        public string Serialize<T>(T dto,string root)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute(root);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), xmlRoot);

            var sb = new StringBuilder();

            using var writer = new StringWriter(sb);

            XmlSerializerNamespaces xmlNamespace = new XmlSerializerNamespaces();

            xmlNamespace.Add(string.Empty, string.Empty);

            xmlSerializer.Serialize(writer,dto,xmlNamespace);

            return sb.ToString();
        }
    }
}
