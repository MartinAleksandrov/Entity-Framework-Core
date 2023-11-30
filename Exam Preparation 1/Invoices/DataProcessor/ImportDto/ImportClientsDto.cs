namespace Invoices.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Client")]
    public class ImportClientsDto
    {

        [XmlElement("Name")]
        [Required]
        [StringLength(25, MinimumLength = 10)]

        public string Name { get; set; } = null!;


        [XmlElement("NumberVat")]
        [StringLength(15, MinimumLength = 10)]

        [Required]
        public string NumberVat { get; set; } = null!;


        [XmlArray("Addresses")]
        public ImportClientWhithAddresses[] Addresses { get; set; } = null!;
    }
}