namespace Trucks.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    using Trucks.Utilities;

    [XmlType("Truck")]
    public class DespatchersTrucks
    {
        [XmlElement("RegistrationNumber")]
        [MinLength(Constance.MaxRegistrationNum)]
        [MaxLength(Constance.MaxRegistrationNum)]
        [RegularExpression(Constance.RegistrationNumberRegex)]
        public string? RegistrationNumber { get; set; }


        [XmlElement("VinNumber")]
        [MinLength(Constance.MaxVinNumLenght)]
        [MaxLength(Constance.MaxVinNumLenght)]
        [Required]
        public string VinNumber { get; set; } = null!;

        //BIG NOTE:Attribute MaxLenght and MinLenght are only for strings and arrays and not for any numeral type!!!
        [XmlElement("TankCapacity")]
        [Range(Constance.MinTankCapacity,Constance.MaxTankCapacity)]
        [Required]
        public int TankCapacity { get; set; }


        [XmlElement("CargoCapacity")]
        [Range(Constance.MinCargoCapacity, Constance.MaxCargoCapacity)]
        [Required]
        public int CargoCapacity { get; set; }


        [XmlElement("CategoryType")]
        [Range(0,3)]
        [Required]
        public int CategoryType { get; set;}


        [XmlElement("MakeType")]
        [Range(0, 4)]
        [Required]
        public int MakeType { get; set;}

    }
}
