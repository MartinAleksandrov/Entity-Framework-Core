namespace Artillery.DataProcessor.ImportDto
{
    using Artillery.Utilities;
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Manufacturer")]
    public class ImportManufacturers
    {
        [XmlElement("ManufacturerName")]
        [Required]
        [MinLength(GlobalConstance.MinManufacturerNameLenght)]
        [MaxLength(GlobalConstance.MaxManufacturerNameLenght)]
        public string ManufacturerName { get; set; } = null!;

        [XmlElement("Founded")]
        [Required]
        [MinLength(GlobalConstance.MinFoundedLenght)]
        [MaxLength(GlobalConstance.MaxFoundedLenght)]
        public string Founded { get; set; } = null!;
    }
}
