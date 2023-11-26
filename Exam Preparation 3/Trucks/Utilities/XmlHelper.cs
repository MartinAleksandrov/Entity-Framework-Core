namespace ProductShop.Utilities
{
    using Newtonsoft.Json;
    using System;
    using System.Text;
    using System.Xml.Serialization;
    using Trucks.DataProcessor.ExportDto;

    public class XmlHelper
    {
        public T Deserialize<T>(string inputXml, string rootName)
        {
            // Serialize + Deserialize

            //Create instance of XMLroot
            var xmlRoot = new XmlRootAttribute(rootName);

            //Serialize to type which we want
            var xmlSerializer = new XmlSerializer(typeof(T), xmlRoot);

            //XmlDeserialize work only whith streams thats why we cast inputXml to certain stream 
            using var reader = new StringReader(inputXml);

            //Desirialize data 
            var dtos = (T)xmlSerializer.Deserialize(reader)!;

            return dtos;
        }

        public string Serialize<T>(T dto, string rootName)
        {
            var serizlizer = new XmlSerializer(typeof(T), new XmlRootAttribute(rootName));

            var sb = new StringBuilder();

            using var writer = new StringWriter(sb);

            var xmlNamespaces = new XmlSerializerNamespaces();

            xmlNamespaces.Add(string.Empty, string.Empty);

            serizlizer.Serialize(writer, dto, xmlNamespaces);

            return sb.ToString();
        }
    }
}
