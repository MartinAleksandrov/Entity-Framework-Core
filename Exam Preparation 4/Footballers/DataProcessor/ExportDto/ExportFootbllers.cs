namespace Footballers.DataProcessor.ExportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    using Footballers.Utilities;

    [XmlType("Footballer")]
    public class ExportFootbllers
    {

        [XmlElement("Name")]
        [MinLength(Constance.MinFootballerNameLenght)]
        [MaxLength(Constance.MaxFootballerNameLenght)]
        public string Name { get; set; } = null!;


        [XmlElement("Position")]
        [Required]
        public string Position { get; set; } = null!; 

    }
}
