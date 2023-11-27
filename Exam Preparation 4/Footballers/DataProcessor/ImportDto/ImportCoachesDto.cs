namespace Footballers.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    using Footballers.Utilities;

    [XmlType("Coach")]
    public class ImportCoachesDto
    {
        [XmlElement("Name")]
        [MinLength(Constance.MinCoachNameLenght)]
        [MaxLength(Constance.MaxCoachNameLenght)]
        [Required]
        public string Name { get; set; } = null!;

        [XmlElement("Nationality")]
        [Required]
        public string Nationality { get; set; } = null!;


        [XmlArray("Footballers")]
        public ImportFootballers[] Footballers { get; set; }

    }
}
