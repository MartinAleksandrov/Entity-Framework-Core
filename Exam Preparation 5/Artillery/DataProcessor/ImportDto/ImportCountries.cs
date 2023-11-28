namespace Artillery.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    using Artillery.Utilities;

    [XmlType("Country")]
    public class ImportCountries
    {
        [XmlElement("CountryName")]
        [Required]
        [MinLength(GlobalConstance.MinCountryNameLenght)]
        [MaxLength(GlobalConstance.MaxCountryNameLenght)]
        public string CountryName { get; set; } = null!;

        [XmlElement("ArmySize")]
        [Required]
        [Range(GlobalConstance.MinArmySize,GlobalConstance.MaxArmySize)]
        public int ArmySize { get; set;}
    }
}
