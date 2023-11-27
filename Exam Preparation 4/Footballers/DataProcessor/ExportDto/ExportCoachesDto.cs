namespace Footballers.DataProcessor.ExportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    using Footballers.Utilities;

    [XmlType("Coach")]
    public class ExportCoachesDto
    {
        [XmlAttribute("FootballersCount")]
        public int Count { get; set; }


        [XmlElement("CoachName")]
        [MinLength(Constance.MinCoachNameLenght)]
        [MaxLength(Constance.MaxCoachNameLenght)]
        [Required]
        public string CoachName { get; set; } = null!;

        [XmlArray("Footballers")]
        public ExportFootbllers[] Footballers { get; set; }
    }
}
