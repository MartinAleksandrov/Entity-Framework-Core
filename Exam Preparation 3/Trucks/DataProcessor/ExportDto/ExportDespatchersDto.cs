namespace Trucks.DataProcessor.ExportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    using Trucks.Utilities;

    [XmlType("Despatcher")]
    public class ExportDespatchersDto
    {
        [XmlAttribute("TrucksCount")]
        public int Count { get; set; }


        [XmlElement("DespatcherName")]
        [MinLength(Constance.MinDespatcherNameLenght)]
        [MaxLength(Constance.MaxDespatcherNameLenght)]
        [Required]
        public string DespatcherName { get; set; } = null!;


        [XmlArray("Trucks")]
        public ExportTrucks[] Trucks { get; set; }
    }
}
