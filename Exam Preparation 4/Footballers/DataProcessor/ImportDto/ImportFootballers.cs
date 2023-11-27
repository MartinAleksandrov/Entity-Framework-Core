namespace Footballers.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    using Footballers.Utilities;

    [XmlType("Footballer")]
    public class ImportFootballers
    {
        [XmlElement("Name")]
        [MinLength(Constance.MinFootballerNameLenght)]
        [MaxLength(Constance.MaxFootballerNameLenght)]
        [Required]
        public string Name { get; set; } = null!;


        [XmlElement("ContractStartDate")]
        [Required]
        public string ContractStartDate { get; set; } = null!;


        [XmlElement("ContractEndDate")]
        [Required]
        public string ContractEndDate { get; set; } = null!; 


        [XmlElement("BestSkillType")]
        [Range(0,4)]
        [Required]
        public int BestSkillType { get; set; }


        [XmlElement("PositionType")]
        [Range(0, 3)]
        [Required]
        public int PositionType { get; set; }

    }
}
