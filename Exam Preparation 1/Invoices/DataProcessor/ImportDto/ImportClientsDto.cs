namespace Invoices.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Client")]
    public class ImportClientsDto
    {

        [XmlElement("Name")]
        [Required]
        [MinLength(10)]
        [MaxLength(25)]
        public string Name { get; set; } = null!;


        [XmlElement("NumberVat")]
        [MinLength(10)]
        [MaxLength(15)]
        [Required]
        public string NumberVat { get; set; } = null!;


        [XmlArray("Addresses")]
        public ImportClientWhithAddresses[] Addresses { get; set; }
    }
}