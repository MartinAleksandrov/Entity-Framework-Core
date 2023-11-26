namespace Trucks.DataProcessor.ExportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    using Trucks.Data.Models.Enums;
    using Trucks.Utilities;

    [XmlType("Truck")]
    public class ExportTrucks
    {
        [XmlElement("RegistrationNumber")]
        [MinLength(Constance.MaxRegistrationNum)]
        [MaxLength(Constance.MaxRegistrationNum)]
        [RegularExpression(Constance.RegistrationNumberRegex)]
        public string? RegistrationNumber { get; set; }


        [Required]
        [XmlElement("Make")]
        [Range(0,4)]
        public MakeType Make { get; set; }
    }
}
