namespace Trucks.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    using Trucks.Utilities;

    [XmlType("Despatcher")]
    public class ImportDespaherDto
    {
        [XmlElement("Name")]
        [MinLength(Constance.MinDespatcherNameLenght)]
        [MaxLength(Constance.MaxDespatcherNameLenght)]
        [Required]
        public string Name { get; set; } = null!;


        [XmlElement("Position")]
        public string? Position { get; set; }


        [XmlArray("Trucks")]
        public DespatchersTrucks[] Trucks { get; set; }
    }
}
